import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import DrockFab from './cmp/fab';
import DrockTopBar from './cmp/drock/topbar';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import './ServerTypeDefs';

/**
 * Construct the home page based on fetched landings
 * @param {Array} landings landings to show on the homepage
 */
function constructHomePage(landings) {
    const homecontainer = $(['div', { class: 'landing-div' }]);
    const homeswiper = $(['drock-swiper',
        {
            class: 'landing-swipe',
            orientation: 'y',
            hidexmove: true
        },
        ['div', { class: 'home-page' },
            ['ul', { class: 'home-page-list' },
                ...landings.map(l => {
                    const ret = $(['li', { class: 'home-page-list-item' },
                        ['a', { class: 'home-page-list-link' },
                            ['div', { class: 'home-page-icons' },
                                ['span', { class: 'home-page-icon-actual material-icons' }, l.icon]
                            ],
                            ['div', { class: 'home-page-listing' },
                                ['div', { class: 'home-page-listing-title' }, l.title],
                                ['div', { class: 'home-page-listing-subtitle' }, l.subtitle]
                            ]
                        ]
                    ]);
                    return ret;
                })
            ]
        ]
    ]);
    homecontainer.append(homeswiper);

    return {
        container: homecontainer,
        swiper: homeswiper
    };
}

/**
 * Construct a landing, including its pages
 * @param {any} landing
 */
function constructLanding(landing) {
    const container = $(['div', { class: 'landing-div' }]);
    const swiper = $(['drock-swiper', {
        class: 'landing-swipe',
        orientation: 'y',
        hidexmove: true
    }]);
    container.append(swiper);

    l.pages.forEach(p => {
        const div = $(['div']);
        const markdown = `# ${p.title}\n\n${p.subtitle}\n\n${p.text || '--'}`;
        const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
        div.innerHTML = html;
        swiper.append(div);
    });

    return {
        container: container,
        swiper: swiper
    };
}

async function fetchhome() {
    let homefetch = await $_.RestFetch("/", "portfolio");

    if (homefetch.landings && homefetch.landings instanceof Array) {
        //Fill the top bar
        /** @type {DrockTopBar} */
        const topbar = $('#drock-main-nav')[0];
        const tabs = [
            { icon: 'home', label: 'Home', active: true },
            ...homefetch.landings
                .map(l => ({ icon: l.icon, label: l.title, active: false, order: l.order }))
                .sort((a, b) => (a.order || 0) - (b.order || 0))
        ];
        topbar.fillTabs(tabs);


        //create the pages
        const pages = [
            constructHomePage(homefetch.landings),
            ...homefetch.landings.map(constructLanding)
        ];
        $("#swipe-base").appendChildren(
            ...pages.map(p => p.container));
    }
}

$(() => {

    $('#drock-contactfab').addEvents({
        click: () => console.log('Clicked contact!')
    });

    fetchhome();
});

let params = new URLSearchParams(document.location.search.substring(1));
var mydata = {
    landing: parseInt(params.get("landing"), 0),
    page: parseInt(params.get("page"), 0)
};
window.onpopstate = function (event) {
    mydata.landing = event.state.landing;
    mydata.page = event.state.page;
    alert("location: " + document.location + ", state: " + JSON.stringify(event.state));
};


