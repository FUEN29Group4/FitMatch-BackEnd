// 获取 input 元素和 img 元素
const inputPhoto = document.getElementById("inputPhoto");
const uploadedPhoto = document.getElementById("uploadedPhoto");

// 监听 input 元素的 change 事件
inputPhoto.addEventListener("change", function (event) {
    const selectedFile = event.target.files[0];

    if (selectedFile) {
        // 将选择的文件作为图片的 src
        uploadedPhoto.src = URL.createObjectURL(selectedFile);
        uploadedPhoto.style.display = "block"; // 显示图片
    }
});

function validateForm() {
    var productCategory = document.getElementById("productCategory").value;
    var productName = document.getElementById("productName").value;
    var productPrice = document.getElementById("productPrice").value;
    var inputPhoto = document.getElementById("inputPhoto").value;

    if (productCategory === "" || productName === "" || productPrice === "" || inputPhoto === "") {
        alert("請填寫所有字段");
        return false;
    }
    return true;
}
