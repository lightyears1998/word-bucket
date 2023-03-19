const remoteUrl = "http://localhost:9123";

browser.contextMenus.create({
  contexts: ["all"],
  id: "menu-save",
  title: "Save selected text to WordBucket"
});

browser.contextMenus.onClicked.addListener((_info, tab) => {
  getSelectedText((text) => makeSaveMaterialRequest(text, tab.title, tab.url));
});

function getSelectedText(cb)
{
  browser.tabs.executeScript({ code: 'window.getSelection().toString();' }, selectedText => cb(selectedText));
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
