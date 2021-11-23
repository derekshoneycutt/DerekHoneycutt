import { Imogene as $_ } from '../Imogene/Imogene';
import RestFetch from '../RestFetch/RestFetch';
import DrockSwiperElement from '../extendcomponents/swiper';
import DrockFab from '../mdccore/fab';
import DrockSplashScreen from '../coreview/splashscreen';
import DrockTopBar from '../coreview/topbar';
import DrockTabBar from '../mdccore/mdctabbar';
import DrockContact from '../coreview/contact';
import DrockMainController from './MainController';
import * as pageFactories from './PageFactories';
import '../ServerTypeDefs';

//Comment this out if not doing
import './appinsights';


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
        pageFactories.constructHomePage(controller, controller.homefetch.landings,
            (index, e) => controller.moveLanding(index + 1, 0, false, true)),
        ...controller.homefetch.landings.map((l, i) => pageFactories.constructLanding(controller, i + 1, l))
    ];

    controller.prepareLandings(tabs, pages);
}

/** Fetch the Home object from the server, which tells us everything to build the initial site!
 * @param {DrockMainController} controller */
async function fetchhome(controller) {
    //localhost:7071 is for development; set otherwise for prod
    let homefetch = await RestFetch("http://localhost:7071/", "api/portfolio");

    controller.homefetch = homefetch;

    if (homefetch.landings && homefetch.landings instanceof Array) {
        fillLandings(controller);
    }

    controller.hideSplash();
}

$_.runOnLoad(() => {
    const controller = new DrockMainController();
    controller.findElements();
    controller.watchUrl();

    fetchhome(controller);
});
