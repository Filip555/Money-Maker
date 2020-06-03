# -*- coding: utf-8 -*-

import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
from sklearn.model_selection import train_test_split

# Importing outputs in form: 3 column first - buy, second - sell, third - none action
dataset_train_y = pd.read_csv('resultsOhlc.csv')
training_set_results = dataset_train_y.iloc[:, 0:3].values

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
for i in range(120, len(training_set)-90):
    #X.append(sc.fit_transform(training_set[i-60:i, 3:8]))
    a = sc.fit_transform(training_set[i-120:i, 3:8])#np.append(sc.fit_transform(training_set[i-120:i, 3:8]), training_set_scaled[i-120:i, 0:3], axis=1)
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

def build_classifier(optimizer='rmsprop',loss='categorical_crossentropy'):
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

regressor = build_classifier('rmsprop','binary_crossentropy')
history = regressor.fit(X_train, y_train, epochs = 500, batch_size = 500, validation_data=(X_test, y_test))

history_dict = history.history
history_dict.keys()

import matplotlib.pyplot as plt

acc= history.history['acc']
val_acc= history.history['val_acc']
loss= history.history['loss']
val_loss= history.history['val_loss']

epochs = range(1, len(acc)+1)

plt.plot(epochs, loss, 'bo', label='Strate trenowania')
plt.plot(epochs, val_loss, 'b', label='Strate walidacji')
plt.title('Strata trenowania i walidacji')
plt.xlabel('Epoki')
plt.ylabel('Strata')

plt.legend()
plt.show()

plt.clf()
acc_values = history_dict['acc']
val_acc_values=history_dict['val_acc']

plt.plot(epochs, acc, 'bo', label='Dokladnosc trenowania')
plt.plot(epochs, val_acc, 'b', label='Dokladnosc walidacji')
plt.title('Dokladnosc trenowania i walidacji')
plt.xlabel('Epoki')
plt.ylabel('Strata')

plt.legend()
plt.show()