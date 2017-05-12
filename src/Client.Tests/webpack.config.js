var path = require("path");
var webpack = require("webpack");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

var babelOptions = {
  presets: [["es2015", {"modules": false}]]//,
  //plugins: ["transform-runtime"]
}

module.exports = {
  entry: resolve('./Client.Tests.fsproj'),
  output: {
    filename: 'tests.js',
    path: resolve('./output'),
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: { 
            plugins: resolve("../../nunit/Fable.Plugins.NUnit.dll"),
            babel: babelOptions
           }
        }
      },
      {
        test: /\.js$/,
        exclude: /node_modules[\\\/](?!fable-)/,
        use: {
          loader: 'babel-loader',
          options: babelOptions
        },
      }
    ]
  },
  resolve:{
    modules:[
      resolve('../Client/node_modules'),
      resolve('./node_modules')]
  }
};
