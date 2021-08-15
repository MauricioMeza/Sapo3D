const mongoose = require('mongoose');

const scoreSchema = new mongoose.Schema(
{
   name:{
       type:String
   },
   pts:{
       type:Number
   }
},
{
    timestamps: true
});

let Score = mongoose.model("Scores", scoreSchema)
module.exports = {Score}