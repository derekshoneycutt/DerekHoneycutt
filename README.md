# Derek Honeycutt

This is a repository for my personal portfolio website and code that I'm willing to show as additional portfolio. Although primarily the codebase for my personal "resume" website, there is some work from other projects included as well. For example, a few C# WPF projects are included in an OTHERS folder; these are generally older code that may be below par on my own standards today, but they provide some useful code and old sampling of work I have collected over the years.

The primary project here is an intentionally over-engineered C# Web API RESTful backend with a vanilla Javascript frontend built with web components. 

Portfolio is currently deployed at [https://www.derekhoneycutt.com](https://www.derekhoneycutt.com)

# Using this Code for Other Projects

This project can be used for other sites with some tweaks. Will likely want to find references to my name that I just carelessly put in code (hopefully that is not much!).

Following this, the following items must be completed:

- Create `webpack.user_config.js`. A demo is available with all options included.
- Create `appsettings.json` and (perhaps) `appsettings.Development.json`. A demo is available with all options included.
- Create `front_src/appinsights.js` or edit `front_src/app.js` to remove reference to it. This is a good spot to include analytic javascript code if any is desired.

# Subprojects

### Imogene

In the front_src directory is a Javascript library called Imogene. This is a diverse library built around DOM manipulations and an automatic client for compatible RESTful interfaces (e.g. the backend included in this project).

Suggested use of importing this library may be like this:
```javascript
import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from 'Imogene';
```

The `$` syntax is remarkable for only kind of following a jQuery type syntax. For example, the following is possible:
```javascript
const checkbox = $("#mycheckbox");
$_.setProperties(arrayOfElements, {
	checked: true
});
```
The `$t` syntax, meanwhile is not largely preferred except to quickly write prototyping code. It is nonetheless included and used. An example:
```javascript
let myval = $_.value("Hello");
const newElement = $(['div', myval]);
const alsoElement = $t`<div>${myval}</div>`;
/* ... later */
myval.set("Goodbye"); /* Updates both elements */
```

The `RestFetch` interface is a wrapper over Javascript's `fetch` that automatically constructs a chain of functions that perform `fetch` based upon hyperlink data. This enables a complete interface to a compatible RESTful API. A compatible API must return JSON that essentially follows this pattern:
```json
{
	...,
	"links": [
		{
			"rel": "This becomes the function name in the JS object",
			"method": "HTTP methods supported, separated by a |",
			"href": "address to query",
			"[postData]": "Any string or object representing a template to send back in POST requests"
		},
		...
	]
}
```
Every link will be translated into a function on the returned object of `RestFetch`, based upon method and rel string. For example, using `RestFetch` may look like this:
```javascript
let data = await $_.RestFetch('/api/', 'portfolio', err_callback);
let settings = await data.getSettings(); 
let postData = Object.assign({}, settings.postAddressPostData);
postData .address = '123 New Address Lane';
await settings.postAddress(postData);
```

### Sawtooth

In the frontend, the sawtooth directory contains a prototype work-in-progress for the frontend of a calendar-based project I have been working with. It is built in here to showcase some of my own code, but it is not more than prototype at this point.

### Components

Included in the `front_src/cmp` directory is several web components that are broadly useful. There are several highly specialized components included in this project, but the primary extendable ones are highlighted here.

##### Floating Action Button (FAB)

This is a really simple component implementing Material Design Components' FAB. The `icon` property identifies what icon to display. Needs to be placed via CSS, but can be treated essentially as a button element.

##### Swiper

This is a component that holds a number of children and enables swiping action upon them. Each child should be held within a `<div>` container for proper function.

The following properties are available:

 - `index` : Sets the child that is currently in view, according to its index, starting at 0. Default value is 0.
 - `hidexmove` : Set to truthy value to hide the hover-over buttons to move in the x axis. Should always be true if orientation is `y`. Default value is falsey.
 - `orientation` : Set to `x` to enable swiping on the x axis, and `y` to enable swiping on the y axis instead. Default value is `x`
 - `allowOvershoot` : If the property is set (e.g. the `allowovershoot` attribute is included in the HTML at all), allows swiping to overshoot, dragging into empty space before snapping back. Otherwise, dragging at ends is static and prevented.

Additionally, the following methods are enabled on the swiping element:

 - `moveToIndex(index, animate = true)` : Moves the swiper to a given index
 - `movePrevious()` : Moves the swiper to a lower index by 1.
 - `moveNext()` : Moves the swiper to a higher index by 1.

The following events are raised upon extra actions on the element:

 - `swipemove` : Raised when the swiper has moved to a different index. Event object includes `detail` object with `index` property set to new index that the swiper lands upon.

##### Calendar

This is a basic component that displays and manages a calendar widget. The user is able to navigate month to month and click on a date to select a given date on the calendar.

The following properties are available:

 - `usemonth` : Any date in a given month to be displayed on the calendar.
 - `selection` : The currently selected date on the calendar.

The following events are raised upon actions on the element:

 - `changemonth` : Raised when the viewed month is changed by user or code.
 - `choosedate` : Raised when the selected date is changed by user or code.


### OTHERS/VagabondLib

This is just a random assortment of C# (heavy on WPF, but useable otherwise) code that can be reused easily enough.

### OTHERS/Gpx

This is a WPF solution that creates a plot of a GPX file to show elevation change over the course of the adventure. This is a rough project not meant for wide consumption, but it is useful, especially for personal use.

##### GpxAnalysis
A simple library that performs basic processing of GPX files. Enables retrieving of many statistics and a basic data structure that is easily deployed to a graph.

##### GpxAnalyzer
WPF project that displays a graph and UI to the processing library. Selection of GPX files can be entered, specific tracks chosen, and overall analysis of these data provided in easily viewable and shareable manner.

### OTHERS/searchicd10

This is an old prototype project that I made to demonstrate the capabilities of a search performed on ICD 10 code.

##### ICD.DataAccess
ICD.DataAccess is a library built for performing searches and lookups on a ICD database built for the searchicd10 project.

##### IcdDatabaseBuilder
Rough (very rough, including in the code!) WPF project for selecting CMS files and building them into a SQLite file. Includes code for other SQL connections, but that has been disabled. Future work might clean this up and make the SQL connections viable to do again.

##### SearchIcd10
Search application prototype, itself. This hasn't aged very well, but it is a WPF application on top of ICD.DataAccess for performing searches and navigating the ICD 10 database. Includes There is some code to login to and maintain a session with Nuance's Nuance Management Console, but this has been disabled. 

# License

This work is hearby released into the public domain.
