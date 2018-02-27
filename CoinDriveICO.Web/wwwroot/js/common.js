$(document).ready(function() {


  $(function(){
    $('.circlestat').circliful();
  });

  $(".cross").click(function () {
    $(".getAddres").hide();
    $(".getRefLink").hide(); 
  });

  $(".btc-address").click(function () {
    getAndSetWalletAddress("BTC");
  });

  $(".eth-address").click(function () {
    getAndSetWalletAddress("ETH");
  });

  $(".getRef").click(function() {
      getReferralLink();
  });

});

function getAndSetWalletAddress(adapterName) {
    $.ajax({
        type: "GET",
        url: "Dashboard/GetInvestAddress",
        data: "adapterName=" + adapterName,
        cache: false,
        success: function(data) {
            $(".wallet-addres").text(data);
            $(".getAddres").show();
        },
        error: function() {
            $(".wallet-addres").text("An error occured while getting your address");
            $(".getAddres").show();
        } 
    });
}

function getReferralLink() {
    $.ajax({
        type: "GET",
        url: "Dashboard/GetAffiliateLink",
        cache: false,
        success: function (data) {
            $(".refLink").attr("href",data);
            $(".getRefLink").show();
        },
        error: function () {
            $("a .refLink").text("An error occured while getting your link");
            $(".getRefLink").show();
        }
    });
}