import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

/** Offset pixels to break into swiping actions */
const barrierOffset = 50;

const preventDefault = e => e.preventDefault();

/** Element that handles swiping */
export default class DrockSwiperElement extends HTMLElement {
    #values = {
        hidexmove: $_.value(this.hidexmove),
        hideymove: $_.value(this.hideymove),
        orientation: $_.value(this.orientation, v => v === 'x' ? 'row' : 'column')
    };
    #elementCache = {
        container: $_.makeEmpty(),
        slot: $_.makeEmpty(),
        prevlink: $_.makeEmpty(),
        uplink: $_.makeEmpty(),
        nextlink: $_.makeEmpty(),
        downlink: $_.makeEmpty()
    };
    /** Whether the swipe originated inside the scrollwith child */
    #swipeInScroll = false;
    /** If applicable, the child elements being scrolled with */
    #childScroll = null;
    /** Whether a child scroller has been allowed to scroll & it is time to swipe! */
    #allowedScroll = false;
    /** Current index being displayed */
    #currIndex = 0;
    /** Number of children recognized in the swipe area */
    #childCount = 0;
    /** Last position of the mouse in a mouse event */
    #lastMouse = 0;

    /**  Whether hover navigation should be allowed to be shown in the component */
    #allowPopoverNav = true;

    /** Whether the component is currently in a swipe operation */
    #isSwiping = false;


    /** Contains the starting point of user's touch that initiated possible swiping */
    #start = null;
    /** Contains measurements taken at start of swiping action, used for maths */
    #startMod = {
        x: 0,
        width: 0,
        y: 0,
        height: 0
    };
    /** Contains bounds & pivot information for movement inside of a swipe */
    #minmax = {
        min: 0,
        max: 0,
        pivot: 0,
        subpivot: 0
    };
    /** Marks if crossed a boundary allowing for swiping actions */
    #crossedBoundary = false;
    /** Marks if user appears to have begun scrolling in opposing axis, blocking swiping actions */
    #beganScroll = false;

    constructor() {
        super();

        /** How much larger than the actual hover icons should the hover be activated by */
        this.hoverMultiplier = 1.5;
        /** Whether swiping is currently allowed within the compononet */
        this.allowSwipe = true;
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-swiper').prop('content').cloneNode(true));

        this.#elementCache = {
            container: shadowRoot.find('.drock-swiper-container'),
            slot: shadowRoot.find('slot'),
            prevlink: shadowRoot.find('.drock-swiper-prev'),
            uplink: shadowRoot.find('.drock-swiper-upprev'),
            nextlink: shadowRoot.find('.drock-swiper-next'),
            downlink: shadowRoot.find('.drock-swiper-downnext')
        };

        this.#elementCache.prevlink.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        this.#elementCache.prevlink.setProperties({
            classList: {
                'drock-swiper-hidden': this.#values.hidexmove
            },
            on: {
                click: this.#onPreviousClick
            }
        });

        this.#elementCache.uplink.forEach(b => {
            b.mdcRipple = new MDCRipple(b);
        });
        this.#elementCache.uplink.setProperties({
            classList: {
                'drock-swiper-hidden': this.#values.hideymove
            },
            on: {
                click: this.#onPreviousClick
            }
        });

        this.#elementCache.nextlink.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        this.#elementCache.nextlink.setProperties({
            classList: {
                'drock-swiper-hidden': this.#values.hidexmove
            },
            on: {
                click: this.#onNextClick
            }
        });

        this.#elementCache.downlink.forEach(b => {
            b.mdcRipple = new MDCRipple(b);
        });
        this.#elementCache.downlink.setProperties({
            classList: {
                'drock-swiper-hidden': this.#values.hideymove
            },
            on: {
                click: this.#onNextClick
            }
        });

        this.#elementCache.container.addEvents({
            mousemove: e => {
                this.#lastMouse = e[`client${this.orientation.toUpperCase()}`];
                this.#updateNavShown();
            },
            mouseleave: e => {
                this.#lastMouse = undefined;
                this.#updateNavShown();
            }
        });

        this.style.setProperty("--drock-swiper-orientation",
            this.orientation === 'x' ? 'row' : 'column');

        this.#elementCache.container[0].addEventListener('touchstart', e => this.#onSwipeBegin(e), true);
        this.#elementCache.container[0].addEventListener('touchmove', e => this.#onSwipeMove(e), true);
        this.#elementCache.container[0].addEventListener('touchend', e => this.#onSwipeEnd(e, true), true);
        this.#elementCache.container[0].addEventListener('touchcancel', e => this.#onSwipeEnd(e, false), true);

        this.#elementCache.slot.addEvents({
            slotchange: e => {
                const nodes = this.#elementCache.slot[0].assignedNodes();
                this.#childCount = nodes.length;
                this.#updateNavShown();
            }
        });
    }

    get index() {
        return this.#currIndex;
    }
    set index(value) {
        this.#setAttribute('index', value);
    }

    get hidexmove() {
        return !!this.getAttribute('hidexmove') || false;
    }
    set hidexmove(value) {
        this.#setAttribute('hidexmove', !!value);
    }

    get hideymove() {
        return !!this.getAttribute('hideymove') || false;
    }
    set hideymove(value) {
        this.#setAttribute('hideymove', !!value);
    }

    /**
     * Describes the current orientation of the swiper (x vs y)
     * @type {String} 
     */
    get orientation() {
        let ret = this.getAttribute("orientation") || "";
        if (ret.toLowerCase().substr(0, 1) === 'y')
            return 'y';
        return 'x';
    }
    set orientation(value) {
        if ((value || "").toLowerCase().substr(0, 1) === 'y')
            this.#setAttribute('orientation', 'y');
        else
            this.#setAttribute('orientation', 'x');
    }

    /** Whether to allow overshooting a swipe and going into the next space
     * @type {Boolean} */
    get allowOvershoot() {
        return !!this.getAttribute("allowovershoot");
    }
    set allowOvershoot(value) {
        this.#setAttribute("allowovershoot", !!value);
    }

    get isSwiping() {
        return this.#isSwiping;
    }

    static get observedAttributes() {
        return ["index", "hidexmove", "hideymove", "orientation", "allowovershoot"];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this.#values[camelName]) {
            this.#values[camelName].set(newValue);
        }

        if (name.toLowerCase() === 'index') {
            let index = Math.trunc(parseInt(this.index, 0));
            if (index !== this.#currIndex)
                this.moveToIndex(index);
        }
    }

    #setAttribute = (attr, value) => {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }


    /**
     * Get the current point of the cursor from an event
     * @param {MouseEvent} evt event to get information from
     * @returns {{x : number, y : number}} current point of the cursor
     */
    #getGesturePointFromEvent = (evt) => {
        var point = {
            x: 0, y: 0
        };

        if (evt.changedTouches && evt.changedTouches[0]) {
            point.x = evt.changedTouches[0].clientX;
            point.y = evt.changedTouches[0].clientY;
        } else {
            point.x = evt.clientX;
            point.y = evt.clientY;
        }

        return point;
    }

    /**
     * Updates the internal minmax for swiping actions
     * @param {number} value value to update the minmax value according to
     */
    #updateminmax = (value) => {
        //First, update min/max if we've crossed them. Tells us where we've been
        if (value < this.#minmax.min)
            this.#minmax.min = value;
        if (value > this.#minmax.max)
            this.#minmax.max = value;

        //PIVOT: Means can start swiping R and then change to swipe L and actually complete it!
            //It's like a preview!
        //Pivot and subpivot circle each other to determine swiping direction
            //Only consider pivoting if passed the standard barrier length away from the present subpivot
        let subpivDiff = value - this.#minmax.subpivot;
        if (Math.abs(subpivDiff) > barrierOffset) {
            let pivDiff = this.#minmax.pivot - this.#minmax.subpivot;
            //If still headed in same direction, update the subpivot to new location, creating new barrier to pivot
            if (Math.sign(subpivDiff) === Math.sign(pivDiff))
                this.#minmax.subpivot = value;
            else {
                //If sufficiently changed direction across the pivot, pivot directions!
                    //This needs special min/max resets as well to reset the new swipe-space
                this.#minmax.pivot = this.#minmax.subpivot;
                this.#minmax.subpivot = value;
                if (subpivDiff > 0) {
                    this.#minmax.min = this.#minmax.pivot;
                    this.#minmax.max = value;
                } else {
                    this.#minmax.max = this.#minmax.pivot;
                    this.#minmax.min = value;
                }
            }
        }
    }

    /**
     * Reset the minmax structure based on a given point
     * @param {({x, y})} detailPoint
     */
    #resetMinMax = (detailPoint) => {
        this.#minmax = {
            min: detailPoint[this.orientation],
            max: detailPoint[this.orientation],
            pivot: detailPoint[this.orientation],
            subpivot: detailPoint[this.orientation] + 1 //because fun (Start out as if heading R)
        };
    }

    /**
     * Event to handle when swiping begins
     * @param {MouseEvent} e event object for beginning swiping
     */
    #onSwipeBegin = (e) => {
        if (!this.allowSwipe)
            return;

        //Note: This doesn't *actually* initiate swiping, though it does prepare the component expecting movement
        const curroffset = this.getBoundingClientRect();
        const detailPoint = this.#getGesturePointFromEvent(e);
        this.#start = detailPoint;
        this.#startMod.width = curroffset.width;
        this.#startMod.height = curroffset.height;
        this.#startMod.x = -1 * curroffset.width * this.#currIndex;
        this.#startMod.y = -1 * curroffset.height * this.#currIndex;
        this.#isSwiping = true;
        this.#resetMinMax(detailPoint);
        this.#crossedBoundary = false;
        this.#beganScroll = false;
        this.#allowPopoverNav = false;

        //Handle children that scroll, if applicable (one at a time: find it?)
        this.#allowedScroll = false;
        const scrollwith = $_.findChildren(this, '.drock-swiper-scrollwith');
        let scrollChildren = [];
        scrollwith.forEach(sw => {
            const divCoords = sw.getBoundingClientRect();

            if (detailPoint.x >= divCoords.left &&
                detailPoint.x <= divCoords.right &&
                detailPoint.y >= divCoords.top &&
                detailPoint.y <= divCoords.bottom) {
                scrollChildren.push(sw);
            }
        });
        if (scrollChildren.length === 1) {
            this.#swipeInScroll = true;
            this.#childScroll = scrollChildren[0];
        }
        else {
            this.#swipeInScroll = false;
            this.#childScroll = null;
        }

        this.#updateNavShown();
        this.style.setProperty('--drock-swiper-slottranslength', '0');
    }

    /**
     * Event for the moues moving during a swiping event
     * @param {MouseEvent} evt event object to process
     */
    #onSwipeMove = (evt) => {
        const antiorient = this.orientation === 'x' ? 'y' : 'x';
        const dimension = this.orientation === 'x' ? 'width' : 'height';
        const capFirst = s => s.charAt(0).toUpperCase() + s.slice(1);

        if (this.#isSwiping && !this.#beganScroll) {
            const detailPoint = this.#getGesturePointFromEvent(evt);
            this.#updateminmax(detailPoint[this.orientation]);

            const moved = detailPoint[this.orientation] - this.#start[this.orientation];
            //Check for a synced up scroller and handle it
            if (this.#swipeInScroll && this.#childScroll && !this.#allowedScroll) {
                if (this.#childScroll[`scroll${capFirst(dimension)}`] >
                    this.#childScroll[`client${capFirst(dimension)}`]) {
                    //We have a scroller we need to keep in check!
                    const scrollprop = this.orientation === 'x' ? 'scrollLeft' : 'scrollTop';
                    if (moved < 1 &&
                        this.#childScroll[`client${capFirst(dimension)}`] +
                        this.#childScroll[scrollprop]
                        < this.#childScroll[`scroll${capFirst(dimension)}`]) {
                        //scrolling down, don't go!
                        return;
                    }
                    else if (moved > 1 && this.#childScroll[scrollprop] > 0) {
                        //scrolling up, don't go!
                        return;
                    }

                    //make the start point now from here, if scrolling is happy!
                    this.#start = detailPoint;
                    this.#resetMinMax(detailPoint);
                    this.#allowedScroll = true;
                    this.#childScroll.addEventListener('touchmove', preventDefault);
                }
            }

            //If not crossed previously, test if we have now and only continue if so
            if (!this.#crossedBoundary) {
                //If sufficiently crossed the standard barrier in the anti position only, block swiping!
                if (Math.abs(detailPoint[antiorient] - this.#start[antiorient]) > barrierOffset) {
                    this.#beganScroll = true;
                    return;
                }

                //Now check if we're far enough to start the swipe!
                if (Math.abs(moved) > barrierOffset)
                    this.#crossedBoundary = true;
                else return;
            }
            evt.cancelable && evt.preventDefault();

            //Move the children, refusing to go beyond the first and last, if appropriate
            let use = this.#startMod[this.orientation] + detailPoint[this.orientation] - this.#start[this.orientation];
            if (!this.allowOvershoot) {
                if (use > 0)
                    use = 0;
                if (use < (this.#childCount - 1) * this.#startMod[dimension] * -1)
                    use = (this.#childCount - 1) * this.#startMod[dimension] * -1;
            }

            this.style.setProperty(`--drock-swiper-positionoffset${this.orientation.toUpperCase()}`, `${use}px`);
        }
    }

    /**
     * Event to run to finish a swiping action
     * @param {MouseEvent} evt event object representing the swipe finish event
     * @param {Boolean} didend Whether this is a true finish to the swiping
     */
    #onSwipeEnd = (evt, didend) => {
        if (this.#isSwiping) {
            this.#isSwiping = false;

            if (this.#allowedScroll) {
                this.#childScroll.removeEventListener('touchmove', preventDefault);
            }
            this.#swipeInScroll = false;
            this.#childScroll = null;
            this.#allowedScroll = false;

            if (evt.cancelable && didend && this.#crossedBoundary)
                evt.preventDefault();

            this.style.setProperty('--drock-swiper-slottranslength', 'var(--drock-transitions-length, 0.3s)');

            //Although difference from start is useful, the pivot allows us some more dynamic use
                //Pivot resets minmaxX object, allowing us to use min/max to figure out swipe direction
            const detailPoint = this.#getGesturePointFromEvent(evt);
            let diff = detailPoint[this.orientation] - this.#minmax.pivot;
            if (diff > 0)
                diff = detailPoint[this.orientation] - this.#minmax.min;
            else
                diff = detailPoint[this.orientation] - this.#minmax.max;

            let moveTo = this.#currIndex;
            if (!this.#beganScroll && Math.abs(diff) > barrierOffset) {
                //Note: You do need to cross back over your starting point to get a full different swipe
                    // Difference between going back to starting child or moving in opposite direction entirely
                let diffFromStart = detailPoint[this.orientation] - this.#start[this.orientation];
                if (diff < 0 && diffFromStart <= 0)
                    ++moveTo;
                else if (diff > 0 && diffFromStart >= 0)
                    --moveTo;
            }
            this.moveToIndex(moveTo);

            this.#start = null;
            this.#allowPopoverNav = true;
            this.#updateNavShown();
        }
    }

    /** Update whether navigation buttons are shown overlaying the viewport */
    #updateNavShown = () => {
        if (this.hideymove) {
            this.#elementCache.uplink[0].classList.toggle('drock-swiper-shownav', false);
            this.#elementCache.downlink[0].classList.toggle('drock-swiper-shownav', false);
        }
        else {
            this.#elementCache.uplink[0].classList.toggle('drock-swiper-shownav', (this.#currIndex < 1));
            this.#elementCache.downlink[0].classList.toggle('drock-swiper-shownav', (this.#currIndex >= this.#childCount - 1));
        }

        if (!this.#allowPopoverNav || this.#lastMouse === undefined) {
            this.#elementCache.prevlink[0].classList.toggle('drock-swiper-shownav', false);
            this.#elementCache.nextlink[0].classList.toggle('drock-swiper-shownav', false);
        }
        else {
            // Get the x-movers by the mouse position only
            let curroffset = this.getBoundingClientRect();
            const prevlinkoffset = this.#elementCache.prevlink[0].getBoundingClientRect();

            const prevEnough = this.orientation === 'x' ?
                curroffset.left + prevlinkoffset.width * this.hoverMultiplier :
                curroffset.top + prevlinkoffset.height * this.hoverMultiplier;
            const nextEnough = this.orientation === 'x' ?
                curroffset.left + curroffset.width - prevlinkoffset.width * this.hoverMultiplier :
                curroffset.top + curroffset.height - prevlinkoffset.height * this.hoverMultiplier;

            this.#elementCache.prevlink[0].classList.toggle('drock-swiper-shownav',
                this.#allowPopoverNav && this.#currIndex > 0 &&
                this.#lastMouse <= prevEnough);
            this.#elementCache.nextlink[0].classList.toggle('drock-swiper-shownav',
                this.#allowPopoverNav && this.#currIndex < this.#childCount - 1 &&
                this.#lastMouse >= nextEnough);
        }
    }

    /**
     * Event to run when a Next button is clicked
     * @param {MouseEvent} evt event object for event
     */
    #onNextClick = (evt) => {
        this.moveNext();
        evt.preventDefault();
    }

    /**
     * Event to run when a Previous button is clicked
     * @param {MouseEvent} evt event object for event
     */
    #onPreviousClick = (evt) => {
        this.movePrevious();
        evt.preventDefault();
    }

    /**
     * Move to a given index of items within the swiping viewport
     * @param {number} index index to move to
     * @param {boolean} animate whether to animate the movement
     */
    moveToIndex = (index, animate = true) => {
        let useIndex = index;
        if (index >= this.#childCount)
            useIndex = this.#childCount - 1;
        if (index < 0)
            useIndex = 0;

        if (this.#currIndex === useIndex)
            return;

        if (!animate)
            this.style.setProperty('--drock-swiper-slottranslength', '0');

        this.#currIndex = useIndex;
        this.style.setProperty(`--drock-swiper-positionoffset${this.orientation.toUpperCase()}`, `-${useIndex}00%`);

        if (!animate)
            this.style.setProperty('--drock-swiper-slottranslength', 'var(--drock-transitions-length, 0.3s)');
        this.#updateNavShown();

        this.index = this.#currIndex;

        this.dispatchEvent(new CustomEvent('swipemove', {
            detail: {
                index: this.#currIndex
            },
            bubbles: true,
            composed: true
        }));
    }

    /** Move to the previous item in the viewport */
    movePrevious = () => {
        this.moveToIndex(this.#currIndex - 1);
    }

    /** Move to the next item in the viewport */
    moveNext = () => {
        this.moveToIndex(this.#currIndex + 1);
    }
}
window.customElements.define('drock-swiper', DrockSwiperElement);
