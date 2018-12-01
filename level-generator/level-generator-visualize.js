require('p5');

const { generate } = require('./level-generator');

const grid = generate(12);

window.setup = () => {
  const scale = 800 / grid.width;

  createCanvas(grid.width * scale + 1, grid.height * scale + 1);
};

window.draw = () => {
  background(255);

  const scale = 800 / grid.width;
  // noStroke();

  for (let i = 0; i < grid.cells.length; i++) {
    const cell = grid.cells[i];

    if (cell.walls[0]) {
      stroke(0);
    } else {
      stroke(240);
    }

    line(cell.x * scale, cell.y * scale, (cell.x + 1) * scale, cell.y * scale);

    if (cell.walls[1]) {
      stroke(0);
    } else {
      stroke(240);
    }

    line(
      (cell.x + 1) * scale,
      cell.y * scale,
      (cell.x + 1) * scale,
      (cell.y + 1) * scale
    );

    if (cell.walls[2]) {
      stroke(0);
    } else {
      stroke(240);
    }

    line(
      (cell.x + 1) * scale,
      (cell.y + 1) * scale,
      cell.x * scale,
      (cell.y + 1) * scale
    );

    if (cell.walls[3]) {
      stroke(0);
    } else {
      stroke(240);
    }

    line(cell.x * scale, (cell.y + 1) * scale, cell.x * scale, cell.y * scale);

    if (cell.blocked) {
      fill(0);
    } else if (cell.start) {
      fill(255, 80);
    } else if (cell.type === 0) {
      fill(0, 255, 0, 80);
    } else if (cell.type === 1) {
      fill(255, 0, 0, 80);
    }

    noStroke();

    rect(cell.x * scale, cell.y * scale, scale, scale);
  }
};
