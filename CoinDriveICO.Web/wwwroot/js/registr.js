$(document).ready(function() {

$( "#registerForm" ).validate({
  rules: {
    username: {
        required: true,
    },
    fullname: {
        required: true,
    },
    email: {
        required: true,
        email: true
    },
    password: {
        required: true,
        minlength: 8,
        pwcheck: true
    },
    passwordConfirm: {
        required: true,
        equalTo: "#confirmPassword",
        pwcheck: true
    },
    terms: {
        required: true
    }
  },
  messages: {
    password: "Must contain at least 8 letters, one number and one capital",
    passwordConfirm: "Passwords must match",
  }
});

$.validator.addMethod("pwcheck", function(value) {
   return /^[A-Za-z0-9\d=!\-@._*]*$/.test(value) // consists of only these
       && /[A-Z]+/.test(value)
       && /\d/.test(value) // has a digit
});


});