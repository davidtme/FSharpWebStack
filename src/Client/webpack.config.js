var path = require("path");
var webpack = require("webpack");

function resolve(filePath) {
  return path.join(__dirname, filePath)
}

var babelOptions = {
  presets: [["es2015", {"modules": false}]],
  plugins: ["transform-runtime"]
}

module.exports = {
  devtool: "source-map",
  entry: resolve('./Client.fsproj'),
  output: {
    filename: 'app.js',
    path: resolve('./wwwroot/scripts'),
    libraryTarget: 'var',
    library: 'App'
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: { babel: babelOptions }
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
    modules:[resolve('./node_modules')],
    alias: {
      //'react': 'react-lite',
      //'react-dom': 'react-lite'
    }
  },
  plugins: [
    //new webpack.DefinePlugin({'process.env': {NODE_ENV: JSON.stringify('production') } })
  ]  
};
