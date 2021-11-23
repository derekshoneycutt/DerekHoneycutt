const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlWebpackPartialsPlugin = require('html-webpack-partials-plugin');

const UserConfig = require('./webpack.user_config').UserConfig;

const buildMode = 'production';

const components = [
    { file: 'extendcomponents/swiper', name: 'swiper' },
    { file: 'mdccore/fab', name: 'fab' },
    { file: 'mdccore/mdctabbar', name: 'mdctabbar' },
    { file: 'coreview/splashscreen', name: 'splashscreen' },
    { file: 'coreview/topbar', name: 'topbar' },
    { file: 'coreview/contact', name: 'contact' },
    { file: 'pages/homepage', name: 'homepage' }
];

const mainPartials = [
    'coreview/splashscreen.html',
    'coreview/topbar.html',
    'coreview/contact.html',
    'extendcomponents/swiper.html',
    'mdccore/fab.html',
    'mdccore/mdctabbar.html',
    'pages/homepage.html'
];

const sawtoothComponents = [
    { file: 'Core/mdciconbutton', name: 'mdciconbutton' },
    { file: 'Views/dayhead', name: 'dayhead' },
    { file: 'Views/week', name: 'week' },
    { file: 'sawbase', name: 'sawbase' }
];

const sawtoothPartials = [
    'fun/calendar.html',
    'cmp/swiper.html',
    'cmp/splashscreen.html',
    'sawtooth/cmp/Core/mdciconbutton.html',
    'sawtooth/cmp/Views/dayhead.html',
    'sawtooth/cmp/Views/week.html',
    'sawtooth/cmp/sawbase.html'
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

function getCssExports(outdir, cmp) {
    return cmp.map(v => ({
        mode: buildMode,
        entry: [`./${v.file}.scss`],
        output: {
            path: path.resolve(__dirname, `wwwroot/${outdir}`),
            filename: `sass.js`
        },
        module: {
            rules: [
                getCSSExportRule(v.name)
            ]
        }
    }));
}

function getHighBodyPartials(templatefilename, paths) {
    return paths.map(v => {
        return {
            path: path.join(__dirname, v),
            priority: "high",
            location: "body",
            template_filename: templatefilename
        };
    });
}

module.exports = [
    ...getCssExports('cmpcss', components),
    {
        mode: buildMode,
        entry: [`./main/app.scss`, './main/app.js'],
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
                title: UserConfig.title,
                first_name: UserConfig.first_name,
                last_name: UserConfig.last_name,
                description: UserConfig.description,
                url: UserConfig.url,
                cssFile: 'bundle.css',
                template: './main/index.html',
                filename: 'index.html'
            }),
            new HtmlWebpackPartialsPlugin(
                getHighBodyPartials('index.html', mainPartials))
        ]
    }/*,
    ...getCssExports('sawtooth/cmp', 'cmpcss/sawtooth', sawtoothComponents),
    {
        mode: buildMode,
        entry: [`./sawtooth/sawtooth.scss`, './sawtooth/sawtooth.js'],
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
                template: './sawtooth/sawtooth.html',
                filename: 'sawtooth.html'
            }),
            new HtmlWebpackPartialsPlugin(
                getHighBodyPartials('sawtooth.html', sawtoothPartials))
        ]
    }*/
];
