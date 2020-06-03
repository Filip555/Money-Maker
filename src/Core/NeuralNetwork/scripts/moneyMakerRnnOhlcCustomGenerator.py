# FOR GPU tips:
# Check if have gpu if not install keras-gpu
# from keras import backend as K
# K.tensorflow_backend._get_available_gpus()

# Importing the libraries
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
from sklearn.model_selection import train_test_split

# Importing outputs in form: 3 column first - buy, second - sell, third - none action
dataset_train_y = pd.read_csv('resultsOhlc.csv')
training_set_results = dataset_train_y.iloc[:, 0:4].values

# Importing the training set - close price and volumen
dataset_train = pd.read_csv('cloePriceOhlc.csv') # bierze pierwsza kolumne jako index
training_set = dataset_train.iloc[:, 0:8].values

# Feature Scaling - normalization - (X-Xmin)/(Xmax-Xmin) close price and volumen
from sklearn.preprocessing import MinMaxScaler
sc = MinMaxScaler(feature_range = (0, 1))
training_set_scaled = sc.fit_transform(training_set[:,0:3])


# Creating a data structure with 60 timesteps and 3 output (Buy,Sell,None)
X = []
y = []
for i in range(120, len(training_set)-15):
    #X.append(sc.fit_transform(training_set[i-60:i, 3:8]))
    a = sc.fit_transform(training_set[i-120:i, 3:8]) #np.append(sc.fit_transform(training_set[i-120:i, 3:8]), training_set_scaled[i-120:i, 0:3], axis=1)
    X.append(a)
   # X.append(training_set_scaled[i-60:i, 0:5])
    
for i in range(0, len(training_set_results)):
    y.append(training_set_results[i, 0:3])

X, y = np.array(X), np.array(y)

# Reshaping make 4 dimension - if u want to add more inputs then increase last property
X = np.reshape(X, (X.shape[0], X.shape[1], 5))

X_train, X_test, y_train, y_test = train_test_split(X, y,test_size=0.2) #20%test data

# Building RNN-LSTM
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from keras.layers import Dropout
from keras.layers import CuDNNLSTM # <- add this library if u want to make calculation on GPU
#from keras.wrappers.scikit_learn import KerasClassifier # <- add this library if u want to testing many parameters
#from sklearn.model_selection import GridSearchCV # <- add this library if u want to testing many parameters

def build_classifier(optimizer='rmsprop',loss='binary_crossentropy'):
    # Initialising the RNN
    regressor = Sequential()
    # Adding the first LSTM layer and some Dropout regularisation
    regressor.add(CuDNNLSTM(units = 50, return_sequences = True, input_shape = (X_train.shape[1], 5))) # Use LSTM on CPU, CuDNNLSTM on GPU
    regressor.add(Dropout(0.2))    
    # Adding a second LSTM layer and some Dropout regularisation
    regressor.add(CuDNNLSTM(units = 50, return_sequences = True))  # Use LSTM on CPU, CuDNNLSTM on GPU
    regressor.add(Dropout(0.2))    
    # Adding a third LSTM layer and some Dropout regularisation
    regressor.add(CuDNNLSTM(units = 50, return_sequences = True))  # Use LSTM on CPU, CuDNNLSTM on GPU
    regressor.add(Dropout(0.2))    
    # Adding a fourth LSTM layer and some Dropout regularisation
    regressor.add(CuDNNLSTM(units = 50))  # Use LSTM on CPU, CuDNNLSTM on GPU
    regressor.add(Dropout(0.2))     
    # Adding the output layer
    regressor.add(Dense(units = 3, activation='softmax'))     
    # Compiling the RNN
    regressor.compile(optimizer = optimizer, loss = loss, metrics=['accuracy']) #'rmsprop'
    return regressor

#### UNCOMMENT THIS CODE TO TESTING MANY PARAMETERS
#regressor = KerasClassifier(build_fn = build_classifier)

#test diffrent parameters

#parameters = {'batch_size': [25, 32],
#              'epochs': [100, 500],
#              'optimizer': ['adam', 'rmsprop'],
#              'loss': ['binary_crossentropy','mean_squared_error']}

#grid_search = GridSearchCV(estimator = regressor,
#                           param_grid = parameters,
#                           return_train_score=True,
#                           scoring='accuracy',
#                           cv = 10,
#                           n_jobs=-1)

#grid_search = grid_search.fit(X_train, y_train)
#best_parameters = grid_search.best_params_
#best_accuracy = grid_search.best_score_
    
#print(pd.DataFrame(grid_search.cv_results_).loc[:, ['mean_test_score', 'rank_test_score']].sort_values(by='rank_test_score'))
    
#### Comment rest code if u are testing many parameters

# Fitting the RNN to the Training set
regressor = build_classifier('rmsprop','binary_crossentropy')
regressor.fit(X_train, y_train, epochs = 500, batch_size = 500)

# Predicted y
ynew = regressor.predict(X_test)

# Checks accuracy to compere them to real data
good_prediction = 0
    
y_test = np.array(y_test)
for i in range(len(X_test)):
    if ((ynew[i,0] > ynew[i,1]) and (ynew[i,0] > ynew[i,2])):
        typeOperationPredict = 'Buy'
    elif ((ynew[i,1] > ynew[i,0]) and (ynew[i,1] > ynew[i,2])):
        typeOperationPredict = 'Sell'
    else:
        typeOperationPredict = 'None'


    if(y_test[i,0] == True and typeOperationPredict=='Buy'): #Buy
        good_prediction += 1
    if(y_test[i,1] == True and typeOperationPredict=='Sell'): #Sell
        good_prediction += 1
    if(y_test[i,2] == True and typeOperationPredict=='None'): #None
        good_prediction += 1

percent_good_prediction = (good_prediction*100) / len(X_test)
print("Accuracy prediction = ", (percent_good_prediction), "%")  


# Save prediciton to csv
import csv

dataset_train_y.iloc[:, 0:4].values

where_to_start = len(y)-len(y_test)
y_result_with_time = dataset_train_y.iloc[where_to_start:,0:4].values
test = np.array(y_result_with_time)

with open('predictionDecision.csv','w') as file:
    for i in range(len(ynew)):
        writer=csv.writer(file)
        if ((ynew[i,0] > ynew[i,1]) and (ynew[i,0] > ynew[i,2])):
            writer.writerow([ynew[i,0],'0','0',test[i,3]])
        elif ((ynew[i,1] > ynew[i,0]) and (ynew[i,1] > ynew[i,2])):
            writer.writerow(['0',ynew[i,1],'0',test[i,3]])
        else:
            writer.writerow(['0','0','1',test[i,3]])

# Remove empty rows
import pandas as pd
df = pd.read_csv('predictionDecision.csv')
df.to_csv('predictionDecision.csv', index=False)

# Score prediciton
scores = regressor.evaluate(X_train, y_train, verbose=0)
print("%s: %.2f%%" % (regressor.metrics_names[1], scores[1]*100))



# saving model
model_json = regressor.to_json()
with open("model.json","w") as json_file:
    json_file.write(model_json)
regressor.save_weights("model.h5")

# to load this model go to other file which is called predictNextMove.py
