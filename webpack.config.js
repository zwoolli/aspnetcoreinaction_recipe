const path = require('path');

module.exports = {
    entry: './src/index.js',
    mode: 'development',
    module: {
        rules: [
            {
                test: /\.css$/, 
                use: ['style-loader', 'css-loader'],
            },
        ]
    },
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'wwwroot', 'dist')
    },
};