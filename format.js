const { exec } = require("child_process");

// remove node binary path from process arguments
const args = process.argv;
args.shift();

const command = `dotnet format --files "${args.join(",")}"`;

exec(command);
