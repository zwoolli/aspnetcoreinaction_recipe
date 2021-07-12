const path = require('path');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = {
    mode: 'development',
    devtool: 'source-map',
    entry: {
        index: './src/js/index.js',
        validation: './src/js/validation.js',
        createRecipe: './src/js/createRecipe.js'
    },
    output: {
        filename: '[name].bundle.js',
        path: path.resolve(__dirname,'wwwroot','dist')
    },
    module: {
        rules: [
            {
                test: /\.css$/, 
                use: ['style-loader', 'css-loader'],
            },
        ]
    },
    plugins: [
        new BundleAnalyzerPlugin({
            analyzerMode: 'disabled',
            generateStatsFile: true,
            statsOptions: { source: false }
        }),
    ],
};