import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import DrockFab from './cmp/fab';
import DrockTopBar from './cmp/drock/topbar';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import './ServerTypeDefs';

const markdown = '# Showdown!\n\n  [Sawtooth](/sawtooth.html)';
const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
$('#enterhere').setProperties({ innerHTML: html });

async function fetchhome() {
    let homefetch = await $_.RestFetch("/", "portfolio");

    if (homefetch.landings && homefetch.landings instanceof Array) {
        /** @type {DrockTopBar} */
        const topbar = $('#drock-main-nav')[0];
        const tabs = [
            { icon: 'home', label: 'Home', active: true },
            ...homefetch.landings
                .map(l => ({ icon: l.icon, label: l.title, active: false, order: l.order }))
                .sort((a, b) => (a.order || 0) - (b.order || 0))
        ];
        topbar.fillTabs(tabs);
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


