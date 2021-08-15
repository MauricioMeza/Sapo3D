const mongoose = require('mongoose')
const dbURL = process.env.DB_URL
console.log(dbURL)

const connect = mongoose.connect(dbURL,  {useNewUrlParser:true, useUnifiedTopology:true}, () => { 
    console.log("mongodb connected at: LocalHost")
    
})

module.exports = connect