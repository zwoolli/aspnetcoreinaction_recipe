const path = require('path');

module.exports = {
    mode: 'development',
    entry: {
        index: './src/js/index.js',
        validation: './src/js/validation.js'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname, 'wwwroot', 'dist')
    },
    optimization: {
        splitChunks: {
            chunks: 'all',
        },
    },
    module: {
        rules: [
            {
                test: /\.css$/, 
                use: ['style-loader', 'css-loader'],
            },
        ]
    },
};