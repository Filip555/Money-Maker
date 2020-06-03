# -*- coding: utf-8 -*-
#conda create --name test2 python=3.7 pandas numpy keras tensorflow keras-gpu Theano matplotlib scikit-learn scipy
from keras.models import model_from_json
import numpy as np
import sys
import pandas as pd
import tensorflow as tf
#print (sys.argv[1:])
#print (sys.argv[0,1])
#gpu_options = tf.GPUOptions(per_process_gpu_memory_fraction=0.3)
#sess = tf.Session(config=tf.ConfigProto(
#  allow_soft_placement=True, log_device_placement=True))


#gpu_options = tf.GPUOptions(per_process_gpu_memory_fraction=0.9)

#tf.Session(config=tf.ConfigProto(gpu_options=gpu_options,allow_soft_placement=True))
# =============================================================================
json_file = open("modelUSDCAD.json",'r')
loaded_model_json = json_file.read()
json_file.close()
loaded_model = model_from_json(loaded_model_json)
loaded_model.load_weights("modelUSDCAD.h5")
# 
loaded_model.compile(loss='binary_crossentropy', optimizer = 'rmsprop', metrics=['accuracy'])
# =============================================================================

# =============================================================================
# from sklearn.preprocessing import minmaxscaler
# sc=minmaxscaler(feature_range=(0,1))
# =============================================================================
# =============================================================================
# 
# =============================================================================
training_set = []
x = np.array(sys.argv[1:])

for i in range(0,x.size):
    arguments = x[i].split("/")
    training_set.append([arguments[0],arguments[1],arguments[2],arguments[3],arguments[4],arguments[5],arguments[6],arguments[7]])
#dataset_train = pd.read_csv('cloePriceOhlc.csv')
#training_set = dataset_train.iloc[:, 0:8].values
#print(training_set)
training_set = np.array(training_set)
# =============================================================================
from sklearn.preprocessing import MinMaxScaler
sc = MinMaxScaler(feature_range = (0, 1))
#training_set_scaled = sc.fit_transform(training_set[:,0:3])
#training_set_scaled = sc.fit_transform(training_set)
# =============================================================================
#print (training_set_scaled)

# =============================================================================

X_train = []
for i in range(120, 121):
    a = sc.fit_transform(training_set[i-120:i, 3:7])#np.append(sc.fit_transform(training_set[i-120:i, 3:8]), training_set_scaled[i-120:i, 0:3], axis=1)
    X_train.append(a)
    #X_train.append(training_set_scaled[i-60:i, 0:5])
X_train = np.array(X_train)

X_train = np.reshape(X_train, (X_train.shape[0], X_train.shape[1],4))
# 
ynew = loaded_model.predict(X_train)
# 
if ((ynew[0,0] > ynew[0,1]) and (ynew[0,0] > ynew[0,2])and ynew[0,0]>0.8):
    print("BUY")
elif ((ynew[0,1] > ynew[0,0]) and (ynew[0,1] > ynew[0,2])and ynew[0,1]>0.8):
    print("SELL")
else:
    print("NONE")
    
print(ynew)


