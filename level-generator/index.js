const path = require('path');
const fs = require('fs');

const { generate } = require('./level-generator');

const grid = generate(6);

const gridFile = path.join(__dirname, 'output', 'grid.json');

fs.writeFile(gridFile, JSON.stringify(grid, null, 2), 'utf8', err => {
  if (err) {
    console.log(err);
  } else {
    console.log(`wrote ${gridFile}`);
  }
});

const cellsFile = path.join(__dirname, 'output', 'cells.json');

fs.writeFile(cellsFile, JSON.stringify(grid.cells, null, 2), 'utf8', err => {
  if (err) {
    console.log(err);
  } else {
    console.log(`wrote ${cellsFile}`);
  }
});
