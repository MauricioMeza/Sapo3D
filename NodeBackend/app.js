var express = require('express');
var app = express();
const port = 3000;

app.use(express.json());

//Initial default Scoreboard
var scoreBoard = [{name:"---", pts:0}, {name:"---", pts:0}, {name:"---", pts:0 }, {name:"---", pts:0}, {name:"---", pts:0}];

//GET the scoreboard from the server
app.get("/score", (req, res) => {
	res.status(200).json({success:true, data: scoreBoard});
});

//POST a new High Score, sorts the scores and finally deletes lowest score
app.put("/addScore", (req, res) => {
	score = req.body;
	console.log(score)
	scoreBoard.push(score);
	scoreBoard.sort((a,b) => {return parseFloat(b.pts) - parseFloat(a.pts)} )
	scoreBoard.pop()
	res.status(200).json({success:true, data: scoreBoard})
})

//Server Initialization (loading the Scores from the Database)
app.listen(port, function() {
  console.log(`Server Connected on Port: ${port}`);

});
