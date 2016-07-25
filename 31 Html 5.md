#HTML5
- a collection of standards, not only html, but javascript and css too.

* new elements and attributes
* new form controls
* support for CSS3
* graphics using canvas
* audio and video integration
* web storage
* local file access
* offline support
* drag and drop support
* microdata

###semantic elements
- article
- header
- footer
- section: separation by content, use div otherwise
- address: 
- aside: content related to the main content
- figure
- figcaption: text below the image
- summary and details
- semantic elements: inline (displayed without a newline first)
    * mark
    * meter
    * progress
    * output
    * time
- nav
- audio and video
- canvas

###css
- tag name: `p { color: green; }`
- class name: `.coffeeType { color: green; }`
- element id: `#siteHeader { color: green; }`
- combined selector: `p.coffeeType { color: yellow; }`
- child selectors: `.coffeetype a { color: red; }`, `.coffeetype > a { color: red; }`
- pseudo classes `a:link {}`, `a:visited {}`, `a:hover {}`, `a:active {}`
- pseudo elements `p:first-child {}`

----------------margin
    ------------border
        --------padding
            ----content

float: browser ignores normal flow to place element at the left or right of the content
clear: browser is not allowed to set elements on left or right of the content

###navigation

`nav`

###input types
`color`, `email`, `number`, `date`, `datetime`, `datetime-local`, `time`, `week`, `month`, `range`, `search`, `tel`, `url`

###attributes
`maxlength`, `pattern`, `min/max`, `placeholder`, `novalidate`, `formnovalidate`, `required`

##css3
###selectors
* attribute selectors
    - starts with: `a[href^="https"]{}`
    - ends with: `a[href$="store"]{}`
    - contains: `a[href*="blog"]{}`
    
* pseudo classes
    - :nth-child `p:nth-child(2n){}` //odd row
    - :nth-last-child `p:nth-last-child(2){}` //second to last
    - :nth-of-type `p:nth-of-type(2n){}` //even row
    - :first-of-type `p:first-of-type{}` //first row
    - :last-child `p:last-child{}` //last child
    - :only-child `p:only-child{}` //only child
    - :not(p) `:not(p)` //element that is not of that type
    - `input:out-of-range {}` 
`:root`, `:empty`, `:target`, `:enabled`, `:disabled`, `:checked`, `:invalid`, `:optional`, `:out-of-range`, `:read-only`, `:read-write`
    
    
* pseudo elements

###borders, backgrounds
`border-*-radius`, `background-size`, `background-origin`, `background (multiple images)`

###gradients
`linear-gradient`, `radial-gradient`

###transforms
`translate`, `rotate`, `scale`, `skew`, `matrix`, `@keyframes`, `animation`, `animation-delay`, `animation-duration`, `animation-fill-mode`, `animation-iteration-count`, `animation-name`, `animation-play-state`, `animation-timing-function`

###page layout: multi-column support
`column-count`, `clumn-width`

##built in libraries
###drag and drop API
`draggable` - attribute true to make an element draggable

- `ondragstart`
- `ondrag`
- `ondragenter`

###web storage
known as local storage or DOM storage

localStorage
- persistent after browser close
- spans browser windows and tabs
- per domain

sessionStorage
- deleted after browser/tab close
- not shared between tabs or browser windows

```
localStorage.setItem("item1", "value to set");
var value = localStorage.getItem("item1");
localStorage.removeItem("item1");
localStorage.clear();
var storageSize = localStorage.length;

window.addEventListener("storage", logMyStorageEvents, false);
/*
    e.key
    e.oldValue
    e.newValue
    e.url
    e.storageArea
*/
```

###geolocation
retrieve geographic location of the user
- latitude
- longitude
- height
- speed

`window.navigator.geolocation`

`watchPosition`: used to listen for changes to position of the user

`clearWatch`: user to stop listening for updates

###video
`src`, `width`, `height`, `poster`, `,uted`, `controls`

multiple sources (for different browsers) using inner `source` tag

video formats: MP4/H.264, WebM, Ogg Theora

audio formats: MP3, AAC, Ogg Vorbis

`paused` - property to indicate media is paused

`autoplay`

###canvas drawing
drawing pane

pixel based (not scalable)

lookless

commonly rendered by the GPU

`canvas`- width and height

`canvas.getContext("2d")`

`fillRect`, `strokeRect`, `clearRect`

###SVG
scalable vector graphics

don't loose quality when zooming

XML based and declarative

has its own DOM

integrates with the surrounding HTML

supports animations

official W3C recommendation

`rect`, `circle`, `ellipse`, `polygon`, `line`, `polylin`, `path`, `text`

define filters using `defs`

####animations
`set`, `animate`, `animateColor`, `animateTransform`, `animateMotion`
