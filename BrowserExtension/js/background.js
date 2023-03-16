const remoteUrl = "http://127.0.0.1:52531";

browser.contextMenus.create({
  contexts: ["all"],
  id: "menu-save",
  title: "Save selected text to WordBucket"
});

browser.contextMenus.onClicked.addListener((info, tab) => {
  console.log(info);
  console.log(tab);
  getSelectedText(makeSaveMaterialRequest);
});

function getSelectedText(cb)
{
  browser.tabs.executeScript({ code: 'window.getSelection().toString();' }, selectedText => cb(selectedText));
}

function makeSaveMaterialRequest(text)
{
  $.ajax({
    type: "POST",
    url: remoteUrl,
    data: { name: "Mary", text },
    success: () => console.log("OK!"),
    dataType: "json"
  });
}
