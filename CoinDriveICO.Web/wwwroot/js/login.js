$(document).ready(function() {


$( "#loginForm" ).validate({
  rules: {
    username: {
        required: true
    },
    password: {
        required: true,
        minlength: 8,
        pwcheck: true
    }
  },
  messages: {
    password: "Must contain at least 8 letters, one number and one capital",
  }
});

});