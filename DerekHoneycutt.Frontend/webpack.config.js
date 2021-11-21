const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const HtmlWebpackPartialsPlugin = require('html-webpack-partials-plugin');

const UserConfig = require('./webpack.user_config').UserConfig;

const buildMode = 'production';

const components = [
    { file: 'swiper', name: 'swiper' },
    { file: 'calendar', name: 'calendar' },
    { file: 'fab', name: 'fab' },
    { file: 'splashscreen', name: 'splashscreen' },
    { file: 'drock/topbar', name: 'topbar' },
    { file: 'drock/contact', name: 'contact' }
];

const mainPartials = [
    'cmp/swiper.html',
    'cmp/fab.html',
    'cmp/splashscreen.html',
    'cmp/drock/topbar.html',
    'cmp/drock/contact.html'
];

const sawtoothComponents = [
    { file: 'Core/mdciconbutton', name: 'mdciconbutton' },
    { file: 'Views/dayhead', name: 'dayhead' },
    { file: 'Views/week', name: 'week' },
    { file: 'sawbase', name: 'sawbase' }
];

const sawtoothPartials = [
    'cmp/calendar.html',
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

function getCssExports(entrydir, outdir, cmp) {
    return cmp.map(v => ({
        mode: buildMode,
        entry: [`./${entrydir}/${v.file}.scss`],
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
    ...getCssExports('cmp', 'cmpcss', components),
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
                /*title: 'Derek Honeycutt',
                first_name: 'Derek',
                last_name: 'Honeycutt',
                description: 'Derek Honeycutt\'s personal portfolio. Software developer, student, photographer, hiker, Subaru Vagabond travel blogger',
                url: 'https://derekhoneycuttportfolio.azurewebsites.net/',*/
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
