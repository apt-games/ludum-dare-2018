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

    switch (cell.type) {
      case 0: // Blocked
        fill(0);
        break;

      case 1: // Start
        fill(255);
        break;

      case 2: // Exit
        fill(255, 0, 255);
        break;

      case 3: // Safe
        fill(0, 255, 0, 200);
        break;

      case 4: // Uncertain Safe
        fill(0, 255, 0, 80);
        break;

      case 5: // Death
        fill(255, 0, 0, 200);
        break;

      case 6: // Uncertain Death
        fill(255, 0, 0, 80);
        break;

      default:
        break;
    }

    noStroke();
    rect(cell.x * scale, cell.y * scale, scale, scale);

    switch (cell.item) {
      case 0: // None
        fill(255, 0);
        break;

      case 1: // Person
        fill(255);
        break;

      case 2: // Item ?
        fill(255, 0, 255);
        break;

      default:
        break;
    }

    noStroke();
    ellipse(cell.x * scale + scale * 0.5, cell.y * scale + scale * 0.5, scale * 0.25);
  }
};
