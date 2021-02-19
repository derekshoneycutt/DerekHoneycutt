import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import DrockFab from './cmp/fab';
import DrockTopBar from './cmp/drock/topbar';
import DrockContact from './cmp/drock/contact';
import DrockMainController from './MainController';
import * as pageFactories from './PageFactories';
import './ServerTypeDefs';


/**
 * Fill the landings
 * @param {DrockMainController} controller
 */
function fillLandings(controller) {
    //Fill the top bar
    const tabs = [
        { icon: 'home', label: 'Home', active: true },
        ...controller.homefetch.landings
            .map(l => ({ icon: l.icon, label: l.title, active: false, order: l.order }))
            .sort((a, b) => (a.order || 0) - (b.order || 0))
    ];

    //create the pages
    const pages = [
        pageFactories.constructHomePage(controller.homefetch.landings,
            (index, e) => controller.moveLanding(index + 1, 0, false, true)),
        ...controller.homefetch.landings.map(pageFactories.constructLanding)
    ];

    controller.prepareLandings(tabs, pages);
}

/** Fetch the Home object from the server, which tells us everything to build the initial site!
 * @param {DrockMainController} controller */
async function fetchhome(controller) {
    let homefetch = await $_.RestFetch("/", "portfolio");

    controller.homefetch = homefetch;

    if (homefetch.landings && homefetch.landings instanceof Array) {
        fillLandings(controller);
    }
}

$(() => {
    const controller = new DrockMainController();
    controller.findElements();
    controller.watchUrl();

    fetchhome(controller);
});
