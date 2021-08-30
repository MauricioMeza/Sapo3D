var express = require('express');
const https = require('https');
const path = require('path');
const fs = require('fs');
const dotenv = require("dotenv");
const cors = require('cors');

dotenv.config();
const { Score } = require("./schema");

const port = process.env.PORT;
const DB = require("./db");
var app = express();
app.use(express.json());
app.use(cors());
app.use(function (req, res, next) {

  res.header('Access-Control-Allow-Origin', "https://mauriciomeza.github.io");
  res.header('Access-Control-Allow-Headers', true);
  res.header('Access-Control-Allow-Credentials', true);
  res.header('Access-Control-Allow-Methods', 'GET, PUT');
  next();
});

//Initial default Scoreboard
var scoreBoard = [{name:"---", pts:0}, {name:"---", pts:0}, {name:"---", pts:0 }, {name:"---", pts:0}, {name:"---", pts:0}];

//GET the scoreboard from the server
app.get("/score", (req, res) => {
	res.status(200).json({success:true, data: scoreBoard});
});

//PUTs a new High Score, sorts the scores and finally deletes lowest score
app.put("/addScore", (req, res) => {
	score = req.body;
	console.log(score);
	scoreBoard.push(score);
	scoreBoard.sort((a,b) => {return parseFloat(b.pts) - parseFloat(a.pts)} )
	scoreDeleted = scoreBoard.pop();

	//Save Score in DB
	const dbScore = new Score(score);
	dbScore.save((err) =>{
		if (!err) {
      		console.log(dbScore);
    	} else {
    		console.log("DB Error");
      		console.log(err);
    	}
	})
	res.status(200).json({success:true, data: scoreBoard})
})

//Setup key and certificate for self-signed SSL
const sslServer = https.createServer({
  key: fs.readFileSync(path.join(__dirname, 'ssl_stuff', 'key.pem') ),
  cert: fs.readFileSync(path.join(__dirname, 'ssl_stuff', 'cert.pem'))
}, app);


//Server Initialization
sslServer.listen(port, () => {
  console.log(`HTTPS Server Connected on Port: ${port}`);
  //Take all scores from the Database and add them to the current score
  Score.find({}, (err, scr) => {
  if (!err) {
        console.log(scr)
        for (var i=0 ; i<scr.length; i++) {
          dbScore = {name:scr[i].name, pts:scr[i].pts}
          scoreBoard.push(dbScore);
          scoreBoard.sort((a,b) => {return parseFloat(b.pts) - parseFloat(a.pts)});
          scoreDeleted = scoreBoard.pop();
        }
    }else{
      console.log("DB Error");
      console.log(err); 
    }
  }); 
})

/*
//Server Initialization (loading the Scores from the Database)
app.listen(port, function() {
  console.log(`Server Connected on Port: ${port}`);
  //Take all scores from the Database and add them to the current score
  Score.find({}, (err, scr) => {
	if (!err) {
        console.log(scr)
        for (var i=0 ; i<scr.length; i++) {
        	dbScore = {name:scr[i].name, pts:scr[i].pts}
        	scoreBoard.push(dbScore);
        	scoreBoard.sort((a,b) => {return parseFloat(b.pts) - parseFloat(a.pts)});
			scoreDeleted = scoreBoard.pop();
        }
  	}else{
  		console.log("DB Error");
  		console.log(err);	
  	}
  });
});
*/