
/** Get a new date object at midnight of the day in this
 * @returns {Date} new Date pointed to midnight */
Date.prototype.getMidnight = function () {
    return new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0, 0);
};
/** Get a new date object pointed to midnight today
 * @returns {Date} new Date pointed to midnight today */
Date.today = function () {
    return new Date(Date.now()).getMidnight();
};
/**
 * Compare how many days exist between two dates
 * @param {Date} date date to compare to
 * @returns {number} number of days between the Dates
 */
Date.prototype.compareDate = function (date) {
    return (new Date(date).getMidnight() - this.getMidnight()) / 86400000;
};
/**
 * Determine if two dates are the same day
 * @param {Date} date date to compare to
 * @returns {boolean} whether the dates are in the same day
 */
Date.prototype.isSameDateAs = function (date) {
    return new Date(date).getMidnight().getTime() - this.getMidnight().getTime();
};
/** Determine if the date is today
 * @returns {boolean} whether the date is today */
Date.prototype.isToday = function () {
    return !this.isSameDateAs(Date.today());
};
/**
 * Get a new Date with n days added
 * @param {number} n Number of days to add to the new Date
 * @returns {Date} New Date with n days added
 */
Date.prototype.plusDays = function (n) {
    let ret = new Date(this);
    ret.setDate(ret.getDate() + n);
    return ret;
};
