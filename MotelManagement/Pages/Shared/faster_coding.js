[document.querySelector("#unregister")].forEach(
    (item)=>{
        document.writeln(item);
        item.preventDefault();
        item.addEventListener(()=>{
            swal({
                title: "Are you sure?",
                text: "Bạn chắc chắn muốn hủy đăng ký phòng này chứ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
            .then((willDelete) => {
                if (willDelete) {
                    window.location.href= item.href;
                }
            });
        })
    }
)
[document.querySelector("#unregister")].forEach(
    (item)=>{
        item.addEventListener("click",(event)=>{
            event.preventDefault();
            swal({
                title: "Are you sure?",
                text: "Bạn chắc chắn muốn hủy đăng ký phòng này chứ?",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
            .then((willDelete) => {
                if (willDelete) {
                    window.location.href= item.href;
                }
            });
        })
    }
)

const img = $(".");
$(".").each(function (index, item) {
    item.addEventListener(("change"),()=>{
        const file = item.files;
        img[index].src = URL.createObjectURL(file);
    })
})
