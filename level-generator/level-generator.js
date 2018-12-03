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

class Graph {
  constructor() {
    this.nodes = [];
    this.graph = {};
    this.end = null;
    this.start = null;
  }

  addNode(node) {
    this.nodes.push(node);

    this.graph[this.createId(node.cell)] = node;
  }

  getNode(id) {
    return this.graph[id];
  }

  createId(cell) {
    return `x:${cell.x},y:${cell.y}`;
  }

  setStart(id) {
    this.start = this.graph[id];
    return this.start;
  }

  setEnd(id) {
    this.end = this.graph[id];
    return this.end;
  }

  reset() {
    for (let i = 0; i < this.nodes.length; i++) {
      this.nodes[i].searched = false;
      this.nodes[i].parent = null;
    }
  }
}

class Node {
  constructor(cell) {
    this.cell = cell;
    this.edges = [];
    this.searched = false;
    this.parent = null;
  }

  addEdge(neighbor) {
    this.edges.push(neighbor);
    neighbor.edges.push(this);
  }
}

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
    const filteredCells = this.cells.filter(cell => cell.type !== 0);

    return filteredCells[randInt(filteredCells.length)];
  }

  getCellByType(type) {
    const filteredCells = this.cells.filter(cell => cell.type === type);

    return filteredCells[randInt(filteredCells.length)];
  }

  getCellAt(x, y) {
    return this.cells[y * this.width + x];
  }

  reset() {
    for (let i = 0; i < this.cells.length; i++) {
      this.cells[i].reset();
    }
  }
}

class Cell {
  constructor(x, y) {
    this.x = x;
    this.y = y;

    this.walls = [1, 1, 1, 1];

    const blocked = simplex.noise2D(x * 0.25, y * 0.25) > 0.35;

    this.type = blocked ? 0 : [3, 4, 5, 6][weightedRandom([10, 50, 10, 30])];
    this.item = blocked ? 0 : [0, 1, 2][weightedRandom([85, 15])];

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

      if (cell) {
        neighborCells.push(cell);
      }
    }

    return neighborCells;
  }

  getNeighborsNotBlocked(grid) {
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

      if (cell && cell.type !== 0) {
        neighborCells.push(cell);
      }
    }

    return neighborCells;
  }

  getRandomAvailableNeighbor(grid) {
    const neighbors = this.getNeighbors(grid).filter(n => !n.visited && n.type !== 0);

    if (neighbors.length > 0) {
      return neighbors[randInt(neighbors.length)];
    }

    return null;
  }

  getRandomNeighborNotVisited(grid) {
    const neighbors = this.getNeighbors(grid).filter(n => !n.visited);

    if (neighbors.length > 0) {
      return neighbors[randInt(neighbors.length)];
    }

    return null;
  }

  getRandomNeighborNotBlocked(grid) {
    const neighbors = this.getNeighbors(grid).filter(n => n.type !== 0);

    if (neighbors.length > 0) {
      return neighbors[randInt(neighbors.length)];
    }

    return null;
  }

  getRandomNeighbor(grid) {
    const neighbors = this.getNeighbors(grid);

    return neighbors[randInt(neighbors.length)];
  }

  addWallsTo(cell) {
    const x = this.x - cell.x;

    if (x === 1) {
      this.walls[3] = 1;
      cell.walls[1] = 1;
    } else if (x === -1) {
      this.walls[1] = 1;
      cell.walls[3] = 1;
    }

    const y = this.y - cell.y;

    if (y === 1) {
      this.walls[0] = 1;
      cell.walls[2] = 1;
    } else if (y === -1) {
      this.walls[2] = 1;
      cell.walls[0] = 1;
    }
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

  reset() {
    this.visited = false;
  }
}

function generate(size = 10) {
  const stack = [];

  const grid = new Grid(size, size);

  const edgeCoords = [ 1, size - 2 ];

  const startX = edgeCoords[randInt(2)];
  const startY = edgeCoords[randInt(2)];

  const start = grid.getCellAt(startX, startY);
  start.type = 1;
  start.item = 0;

  let exit = grid.pickRandomCell();
  let endTries = 0;

  const distance = (a, b) => {
    const dx = a.x - b.x, dy = a.y - b.y;

		return Math.sqrt( dx * dx + dy * dy);
  }

  while (distance(start, exit) < size - 1.5) {
    exit = grid.pickRandomCell();
    endTries++;
  }

  console.log(`Tried ${endTries} times before we found exit.`);

  exit.type = 2;
  exit.item = 0;

  const graph = new Graph();

  for (let i = 0; i < grid.cells.length; i++) {
    const cell = grid.cells[i];

    const node = new Node(cell);
    graph.addNode(node);
  }

  for (let i = 0; i < grid.cells.length; i++) {
    const cell = grid.cells[i];

    const cellNode = graph.getNode(graph.createId(cell))

    const neighbors = cell.getNeighbors(grid);

    for (let j = 0; j < neighbors.length; j++) {
      const neighbor = neighbors[j];

      const neighborNode = graph.getNode(graph.createId(neighbor));

      cellNode.addEdge(neighborNode);
    }
  }

  function bfs() {
    const startNode = graph.setStart(graph.createId(start));
    const endNode = graph.setEnd(graph.createId(exit));

    const queue = [];

    startNode.searched = true;
    queue.push(startNode);

    while (queue.length > 0) {
      const current = queue.shift();

      if (current === endNode) {
        console.log("Found " + current.value);
        break;
      }

      const edges = current.edges;

      for (let i = 0; i < edges.length; i++) {
        var neighbor = edges[i];

        if (!neighbor.searched) {
          neighbor.searched = true;
          neighbor.parent = current;
          queue.push(neighbor);
        }
      }
    }

    const path = [];

    path.push(endNode);

    let next = endNode.parent;

    while (next !== null) {
      path.push(next);
      next = next.parent;
    }

    return path;
  }

  const shortestPath = bfs();

  for (let i = 0; i < shortestPath.length; i++) {
    const node = shortestPath[i];

    if (node.cell.type !== 1 && node.cell.type !== 2) {
      node.cell.type = i > shortestPath.length - 4 ? 4 : [3, 4, 5, 6][weightedRandom([10, 50, 10, 30])];
      // node.cell.type = [3, 4, 5, 6][weightedRandom([10, 50, 10, 30])];
      // node.cell.type = 7;
    }
  }

  for (let i = 0; i < grid.cells.length; i++) {
    // const cell = grid.pickRandomCell();
    const cell = grid.cells[i];

    const neighbors = cell.getNeighbors(grid);

    for (let j = 0; j < neighbors.length; j++) {
      const neighbor = neighbors[j];

      if (cell.type === 0) {
        cell.addWallsTo(neighbor);
      } else if (neighbor.type !== 0) {
        cell.removeWallsTo(neighbor);
      }

    }
  }

  // const exit = grid.pickRandomCell();


  // grid.reset();
  // // console.log(start);
  // current = start;
  // let foundExit = false;

  // while (current) {
  //   if (foundExit) {
  //     const prev = stack.pop();

  //     if (prev && prev !== start) {
  //       prev.type = 7;
  //       prev.blocked = false;
  //       current.removeWallsTo(prev);
  //     }

  //     current = prev;
  //   } else {
  //     const next = current.getRandomNeighborNotVisited(grid);

  //     if (next) {
  //       next.visited = true;

  //       stack.push(current);

  //       current = next;

  //       if (next.type === 2) {
  //         foundExit = true;
  //       }
  //     } else {
  //       const prev = stack.pop();

  //       current = prev;
  //     }
  //   }
  // }

  return grid;
}

module.exports = {
  Grid: Grid,
  Cell: Cell,
  generate,
};
