$(document).ready(function() {

$( "#resetForm" ).validate({
  rules: {
    email: {
      required: true,
      email: true
    }
  },
});

});