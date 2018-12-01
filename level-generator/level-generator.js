const fs = require('fs');
const SimplexNoise = require('simplex-noise');

const simplex = new SimplexNoise();

const randBetween = (min, max) => min + Math.random() * (max - min);
const rand = n => randBetween(0, n);
const randInt = n => Math.floor(rand(n));

const weightedRandom = weights => {
  let totalWeight = weights.reduce((acc, curr) => (acc += curr));
  let random = Math.random() * totalWeight;

  for (let i = 0; i < weights.length; i++) {
    if (random < weights[i]) {
      return i;
    }

    random -= weights[i];
  }

  return 0;
};

class Grid {
  constructor(width, height) {
    this.width = width;
    this.height = height;

    this.cells = [];

    for (let y = 0; y < this.height; y++) {
      for (let x = 0; x < this.width; x++) {
        this.cells.push(new Cell(x, y));
      }
    }
  }

  pickRandomCell() {
    const filteredCells = this.cells.filter(cell => !cell.blocked);

    return filteredCells[randInt(filteredCells.length)];
  }

  getCellByType(type) {
    const filteredCells = this.cells.filter(cell => cell.type === type);

    return filteredCells[randInt(filteredCells.length)];
  }

  reset() {
    for (let i = 0; i < this.cells.length; i++) {
      const cell = this.cells[i];

      cell.visited = false;
    }
  }
}

class Cell {
  constructor(x, y) {
    this.x = x;
    this.y = y;

    this.start = false;
    this.exit = false;

    this.walls = [1, 1, 1, 1];
    this.blocked = simplex.noise2D(x * 0.1, y * 0.1) > 0.6;

    if (!this.blocked) {
      this.type = [0, 1][weightedRandom([60, 40])];
    }

    this.visited = false;
  }

  setType(type) {
    this.type = type;
  }

  getNeighbors(grid) {
    const neighbors = [
      [this.x - 1, this.y],
      [this.x + 1, this.y],
      [this.x, this.y - 1],
      [this.x, this.y + 1],
    ].filter(([x, y]) => x >= 0 && x < grid.width && y >= 0 && y < grid.height);

    const neighborCells = [];

    for (let i = 0; i < neighbors.length; i++) {
      const [x, y] = neighbors[i];
      const cell = grid.cells[y * grid.width + x];

      if (cell && !cell.blocked) {
        neighborCells.push(cell);
      }
    }

    return neighborCells;
  }

  getRandomAvailableNeighbor(grid) {
    const neighbors = this.getNeighbors(grid).filter(n => !n.visited);

    if (neighbors.length > 0) {
      return neighbors[randInt(neighbors.length)];
    }

    return null;
  }

  getRandomNeighbor(grid) {
    const neighbors = this.getNeighbors(grid);

    return neighbors[randInt(neighbors.length)];
  }

  removeWallsTo(cell) {
    const x = this.x - cell.x;

    if (x === 1) {
      this.walls[3] = 0;
      cell.walls[1] = 0;
    } else if (x === -1) {
      this.walls[1] = 0;
      cell.walls[3] = 0;
    }

    const y = this.y - cell.y;

    if (y === 1) {
      this.walls[0] = 0;
      cell.walls[2] = 0;
    } else if (y === -1) {
      this.walls[2] = 0;
      cell.walls[0] = 0;
    }
  }
}

function generate() {
  const stack = [];

  const grid = new Grid(20, 20);

  let didSetExit = false;

  let current = grid.pickRandomCell();
  current.start = true;
  // let current = grid.cells[0];
  current.visited = true;

  while (current) {
    const next = current.getRandomAvailableNeighbor(grid);

    if (next) {
      next.visited = true;

      stack.push(current);

      current.removeWallsTo(next);

      current = next;
    } else {
      // if (!didSetExit) {
      //   current.exit = true;
      //   didSetExit = true;
      // }

      const prev = stack.pop();

      current = prev;
    }
  }

  for (let i = 0; i < Math.floor(grid.cells.length / 2); i++) {
    const cell = grid.pickRandomCell();
    const neighbor = cell.getRandomNeighbor(grid);

    if (neighbor) {
      cell.removeWallsTo(neighbor);
    }
  }

  return grid;
}

const grid = generate();

fs.writeFile('./grid.json', JSON.stringify(grid, null, 2), 'utf8', err => {
  if (err) {
    console.log(err);
  } else {
    console.log('wrote grid.json');
  }
});
fs.writeFile(
  './cells.json',
  JSON.stringify(
    grid.cells.map(({ x, y, start, blocked, walls, type }) => ({
      x,
      y,
      start,
      blocked,
      walls,
      type,
    })),
    null,
    2
  ),
  'utf8',
  err => {
    if (err) {
      console.log(err);
    } else {
      console.log('wrote cells.json');
    }
  }
);
