﻿gallery_set = [

]
jQuery(document).ready(function () {
	"use strict";
	document.getElementById("footter-main").style.display = "none";
	getData();
	header.addClass('fixed_header');
	jQuery('body').fs_gallery({
		fx: 'fade', /*fade, zoom, slide_left, slide_right, slide_top, slide_bottom*/
		fit: 'no_fit',
		slide_time: 3000, /*This time must be < then time in css*/
		autoplay: 1,
		show_controls: 1,
		slides: gallery_set
	});
	jQuery('.fs_share').on("click", function () {
		jQuery('.fs_fadder').removeClass('hided');
		jQuery('.fs_sharing_wrapper').removeClass('hided');
		jQuery('.fs_share_close').removeClass('hided');
	});
	jQuery('.fs_share_close').on("click", function () {
		jQuery('.fs_fadder').addClass('hided');
		jQuery('.fs_sharing_wrapper').addClass('hided');
		jQuery('.fs_share_close').addClass('hided');
	});
	jQuery('.close_controls').on("click", function () {
		html.toggleClass('hide_controls');
	});

	jQuery('.fs_controls_append').prepend('<a href="javascript:void(0)" class="post_info hasContent"></a>');
	jQuery('.custom_bg').remove();
	jQuery('.main_header').removeClass('hided');
	jQuery('html').addClass('single-gallery');
	jQuery('html').addClass('without_thmb');
	jQuery('.share_toggle').on("click", function () {
		jQuery('.share_block').toggleClass('show_share');
	});

	jQuery('.post_info').on("click", function () {
		if (!jQuery(this).hasClass('noContent')) {
			html.toggleClass('show_content');
		}
	});
});
function getData() {
	$.ajax({
		url: '/api/GetProject?url=' + window.location.pathname,
		type: 'Get',
		dataType: 'Json',
		contentType: 'application/json',
		success: function (data) {
			gallery_set = data.data;
		},
		error: function (x) { },
		async: false
	});
}