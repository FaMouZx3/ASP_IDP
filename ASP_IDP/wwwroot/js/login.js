var logoFadeSleep = 1000;

$(document).ready(function () {
	$("#login-logo").delay(logoFadeSleep).fadeOut().next().delay(logoFadeSleep + 1000).fadeIn("slow").promise();
});