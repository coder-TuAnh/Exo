	
/*Scroll page*/
window.onscroll = function() {scrollFunction()};
function scrollFunction() {
	if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
		document.getElementById("navbar").classList.add("bg-white");
	} else {
		document.getElementById("navbar").classList.remove("bg-white");
	}
}
/**/
function myFunction() {
  document.getElementById("myDropdown").classList.toggle("show");
}
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {
    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}

/* load list view and gird view */
var elements = document.getElementsByClassName("item-tour");
var i;

function listView() {
  for (i = 0; i < elements.length; i++) {
    elements[i].style.width = "100%";
    elements[i].classList.add("inline-item");
  }
}
function gridView() {
  for (i = 0; i < elements.length; i++) {
    elements[i].style.width = "33.33%";
    elements[i].classList.remove("inline-item");
  }
}
var btns = document.getElementsByClassName("bt-view");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function() {
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}


/* fancybox */

$(document).ready(function() {
  $('.fancybox').fancybox();
});


