const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');

const components = [
    { file: 'swiper', name: 'swiper' },
    { file: 'calendar', name: 'calendar' }
];
const sawtoothComponents = [
    { file: 'Core/mdciconbutton', name: 'mdciconbutton' },
    { file: 'Views/dayhead', name: 'dayhead' },
    { file: 'Views/week', name: 'week' },
    { file: 'sawbase', name: 'sawbase' },
    { file: 'splashscreen', name: 'splashscreen' }
];

function getCSSExportRule(name) {
    return {
        test: /\.scss$/,
        use: [
            {
                loader: 'file-loader',
                options: {
                    name: `${name}.css`
                }
            },
            { loader: 'extract-loader' },
            {
                loader: 'css-loader',
                options: { url: false, sourceMap: true }
            },
            {
                loader: 'postcss-loader'
            },
            {
                loader: 'sass-loader',
                options: {
                    // Prefer Dart Sass
                    implementation: require('sass'),

                    // See https://github.com/webpack-contrib/sass-loader/issues/804
                    webpackImporter: false,
                    sassOptions: {
                        includePaths: ['./node_modules']
                    }
                }
            }
        ]
    };
}

function getCSSExportsForComponents() {
    return components.map(v => ({
        mode: 'production',
        entry: [`./front_src/cmp/${v.file}.scss`],
        output: {
            path: path.resolve(__dirname, 'wwwroot/cmpcss'),
            filename: `sass.js`
        },
        module: {
            rules: [
                getCSSExportRule(v.name)
            ]
        }
    }));
}

function getCSSExportsForSawtoothComponents() {
    return sawtoothComponents.map(v => ({
        mode: 'production',
        entry: [`./front_src/sawtooth/cmp/${v.file}.scss`],
        output: {
            path: path.resolve(__dirname, 'wwwroot/cmpcss/sawtooth'),
            filename: `sass.js`
        },
        module: {
            rules: [
                getCSSExportRule(v.name)
            ]
        }
    }));
}

module.exports = [
    ...getCSSExportsForComponents(),
    ...getCSSExportsForSawtoothComponents(),
    {
        mode: 'production',
        entry: ['./front_src/app.scss', './front_src/app.js'],
        output: {
            path: path.resolve(__dirname, 'wwwroot'),
            filename: 'bundle.js'
        },
        module: {
            rules: [
                {
                    test: /\.js?$/
                },
                getCSSExportRule('bundle')
            ]
        },
        plugins: [
            new HtmlWebpackPlugin({
                hash: true,
                title: 'Derek Honeycutt',
                cssFile: 'bundle.css',
                template: './front_src/index.html',
                filename: 'index.html'
            })
        ]
    },
    {
        mode: 'production',
        entry: ['./front_src/sawtooth/sawtooth.scss', './front_src/sawtooth/sawtooth.js'],
        output: {
            path: path.resolve(__dirname, 'wwwroot'),
            filename: 'sawtooth.js'
        },
        module: {
            rules: [
                {
                    test: /\.js?$/
                },
                getCSSExportRule('sawtooth')
            ]
        },
        plugins: [
            new HtmlWebpackPlugin({
                hash: true,
                title: 'Sawtooth',
                cssFile: 'sawtooth.css',
                template: './front_src/sawtooth/sawtooth.html',
                filename: 'sawtooth.html'
            })
        ]
    }
];
