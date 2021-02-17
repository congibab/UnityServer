const express = require('express'); 
const app = express() 
app.get('/', (req, res) => res.send('Hello World test')); 

app.listen(3000, () => { 
	console.log('My REST API running on port 3000!');
})
