const remoteUrl = "http://localhost:9123";

browser.contextMenus.create({
  contexts: ["all"],
  id: "menu-save",
  title: "Save selected text to WordBucket"
});

browser.contextMenus.onClicked.addListener((info, tab) => {
  getSelectedText(info, (text) => makeSaveMaterialRequest(text, tab.title, tab.url));
});

function getSelectedText(info, cb)
{
  var selectedText = info.selectionText;
  // browser.tabs.executeScript({ code: 'window.getSelection().toString();' }, selectedText => cb(selectedText));
  cb(selectedText);
}

function makeSaveMaterialRequest(text, title, uri)
{
  $.ajax({
    type: "POST",
    url: remoteUrl,
    data: JSON.stringify({
      title: String(title),
      uri: String(uri),
      text:  Array(text).join("\n")
    }),
    success: () => console.log("OK!"),
    contentType:"application/json; charset=utf-8"
  });
}
