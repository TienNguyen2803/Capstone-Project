//  function saveDocument(_callback) {
//      // TXTextControl.saveDocument(TXTextControl.StreamType.WordprocessingML, function(e) {
//      //     console.log(e.data);
//      // });
//      TXTextControl.saveDocument(TXTextControl.StreamType.WordprocessingML, function(e) {
//          _callback(e.data);

//      });

//  }

//  function remove() {
//      TXTextControl.resetContents();

//  }

//  function WordtoHtml(input) {


//      document.getElementById("html-viewer").innerHTML = input;

//      $(".draggable").draggable({
//          revert: true
//      });
//      $(".draggable").css('z-index', 1000);
//      $("#html-viewer p").css('z-index', 900);
//      //$("#html-viewer p").css('height', '50px');
//      $("#html-viewer table").css('border-collapse', 'collapse');
//      $("#html-viewer table").css('border', '1px solid black');
//      $("#html-viewer table").css('width', '100%');
//      $("#html-viewer table td").css('border', '1px solid black');
//      $("#html-viewer table td").css('min-width', '50px');
//      $("#html-viewer table td").css('min-height', '50px');
//      $("#html-viewer table th").css('border', '1px solid black');

//      $("#html-viewer p").each(function() {
//          if ($(this).find(".fixed").length === 0) {
//              $(this).html('<span class="fixed" style="display:inline-block">' + $(this).html() + '</span>');
//          }
//      })

//      $("#html-viewer p, #html-viewer td").droppable({
//          drop: function(event, ui) {
//              let htmlText = $(ui.draggable[0]).html();
//              let htmlTemp;

//              if (event.target.tagName === 'P') {
//                  htmlTemp = ($(this).html() == '' || $(this).html() == '&nbsp;') ? htmlText : $(this).html() + ' ' + htmlText;

//                  if ($(this).find('.fixed').length === 0) {
//                      $(this).html('<span class="fixed" style="display:inline-block">' + $(this).html() + '</span>');
//                  }

//                  if ($(this).find(".field").length === 0) {
//                      $(this).append('<span class="field" style="display:inline-block">' + htmlText + '</span>');
//                  } else {
//                      $(this).find(".field").html(htmlText);
//                  }
//              } else if (event.target.tagName === 'TD') {
//                  htmlTemp = htmlText;

//                  $(this).html(htmlTemp);
//              } else if (event.target.tagName === 'SPAN') {
//                  console.log('asdasd');
//              }

//          }
//      });

//      $('#html-viewer p').on('click', function(event) {
//          if (event.target.tagName !== 'INPUT') {
//              //  if ($('.fixed.active').length !== 0) {
//              //      $('.fixed.active').html($('.fixed.active input').val());
//              //      $('.fixed.active').removeClass('active');
//              //  } else {

//              //  }

//              if ($('.fixed.active').length === 0) {
//                  $(this).find('.fixed').addClass('active');

//                  let fixedObject;

//                  if ($(this).find(".fixed").children().length === 0) {

//                      fixedObject = $(this).find('.fixed');
//                      $('.fixed.active').attr("children", "off")
//                  } else {

//                      fixedObject = $($(this).find('.fixed').children()[0]);
//                      $('.fixed.active').attr("children", "on")
//                  }


//                  const fixedHtml = fixedObject.html();

//                  fixedObject.html(`<input value="${fixedHtml}" style="padding:0;letter-spacing:1px" />`);
//              }
//          }
//      })

//      $(document).on('keypress', function(e) {
//          if (e.which == 13) {
//              let fixedObject;

//              if ($(".fixed.active").attr("children") === "off") {
//                  fixedObject = $('.fixed.active');
//              } else {
//                  fixedObject = $($('.fixed.active').children()[0]);
//              }

//              fixedObject.html(fixedObject.find('input').val());
//              $('.fixed.active').removeClass('active');
//          }
//      });
//  }

//  function loadDocument(input) {
//      var x = input.name;
//      //input = input.srcElement;
//      //if (input.files && input.files[0]) {
//      var fileReader = new FileReader();
//      fileReader.onload = function(e) {

//          var streamType = TXTextControl.streamType.PlainText;

//          // set the StreamType based on the lower case extension
//          switch (x.split('.').pop().toLowerCase()) {
//              case 'doc':
//                  streamType = TXTextControl.streamType.MSWord;
//                  break;
//              case 'docx':
//                  streamType = TXTextControl.streamType.WordprocessingML;
//                  break;
//              case 'rtf':
//                  streamType = TXTextControl.streamType.RichTextFormat;
//                  break;
//              case 'html':
//                  streamType = TXTextControl.streamType.HTMLFormat;
//                  break;
//              case 'tx':
//                  streamType = TXTextControl.streamType.InternalUnicodeFormat;
//                  break;
//              case 'pdf':
//                  streamType = TXTextControl.streamType.AdobePDF;
//                  break;
//          }

//          // load the document beginning at the Base64 data (split at comma)
//          TXTextControl.loadDocument(streamType, e.target.result.split(',')[1]);
//      };

//      // read the file and convert it to Base64
//      fileReader.readAsDataURL(input);
//      //}
//  }

//  function addMergeField() {
//      let mergeField = new TXTextControl.MergeField();
//      const fieldName = document.getElementById('field-name').value;

//      mergeField.name = fieldName;
//      mergeField.text = ` [${fieldName}] `;

//      TXTextControl.addMergeField(mergeField);
//  }

//  function load() {
//      TXTextControl.addEventListener('textControlLoaded', function() {
//          loadDocument();
//      });
//  }
//  window.onload = function() {
//      TXTextControl.addEventListener('textControlLoaded', function() {
//          loadDocument();
//      });

//  };