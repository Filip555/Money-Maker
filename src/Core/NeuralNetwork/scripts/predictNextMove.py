from keras.models import model_from_json
import numpy as np
import sys

print(sys.argv[1:])
# =============================================================================
# 
# # load json and create model
# json_file = open('model.json', 'r')
# loaded_model_json = json_file.read()
# json_file.close()
# loaded_model = model_from_json(loaded_model_json)
# # load weights into new model
# loaded_model.load_weights("model.h5")
# 
# 
# # compile loaded model
# loaded_model.compile(loss='binary_crossentropy', optimizer='rmsprop', metrics=['accuracy'])
# 
# from sklearn.preprocessing import MinMaxScaler
# sc = MinMaxScaler(feature_range = (0, 1))
# training_set_scaled = sc.fit_transform(training_set)
# 
# # Get only one input with 60 vectors
# X_test = []
# for i in range(0, 1):
#     X_test.append(training_set_scaled[i-60:i, 0:2])
#     
# X_test = np.array(X_test)
# X_test = np.reshape(X_test, (X_test.shape[0], X_test.shape[1], 2))
# 
# # Predicted y
# ynew = loaded_model.predict(X_test)
# 
# 
# print()
# 
# 
# # =============================================================================
# # 
# # score = loaded_model.evaluate(X_train, y_train, verbose=0)
# # print("%s: %.2f%%" % (loaded_model.metrics_names[1], score[1]*100))
# # =============================================================================
# 
# =============================================================================
