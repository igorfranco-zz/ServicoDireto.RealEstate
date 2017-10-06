var mongoose = require('mongoose');
var mongoUrl = 'mongodb://192.168.1.102:27017/crawler';

mongoose.connect(mongoUrl, { useMongoClient: true, promiseLibrary: global.Promise });

var Schema = mongoose.Schema,
ObjectId = Schema.ObjectId;

var ResultStatusSchema = new Schema({
    message : String,
    status  : { type: String, enum: ['error', 'success'] }
});

//class
var ResultStatusModel = mongoose.model('ResultStatusModel', ResultStatusSchema);
var resultStatus = new ResultStatusModel( { message : "mundao", status: "success" });

resultStatus.save(function (err) {
    if (err) {
      console.log(err);
    } else {
      console.log('inserted');
    }
  });

  ResultStatusModel.findOne({ status: 'success' }, function(err, record) {
    console.log(record); 
});