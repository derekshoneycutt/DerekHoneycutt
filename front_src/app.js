import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import './ServerTypeDefs';

const markdown = '# Showdown!\n\n  [Sawtooth](/sawtooth.html)';
const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
$('#enterhere').setProperties({ innerHTML: html });

async function fetchhome() {
    let homefetch = await $_.RestFetch("/", "portfolio");
    console.log(homefetch);
}
fetchhome();

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


