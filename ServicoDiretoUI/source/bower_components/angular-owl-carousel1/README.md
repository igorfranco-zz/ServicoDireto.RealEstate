# angular-owl-carousel1

Angular directive wrapping owl carousel v1

*For use with Owl Carousel 1*

## Usage

Javascript:
```javascript

	// Define app module
	angular
		.module('myApp', ['angular-owl-carousel']);

	// Create controller
	angular
		.module('myApp')
		.controller('MyController', MyController);

	function MyController() {
		this.items = ['item1', 'item2'];
	}
```

HTML:
```html
	<div ng-controller="MyController">
		<div
			id="myCarousel"
			items="1"
			pagination="true"
			owl-carousel="items">
			<div class="item">{{ item }}</div>
		</div>
	</div>
	<button ng-click="owlCarousel.myCarousel.next()">
		Next
	</button>
```

Feel free to use owl carousel params described on [official site](http://owlgraphic.com/owlcarousel/#customizing). Just add them as on the code example above.

### New in version 1.0.1:

1. Add `id` for carousel to be able to call entity's methods.

*Note:* I created this directive for owl carousel v1 because v2 is in beta now and has some problems with destroying that I could not fight.
