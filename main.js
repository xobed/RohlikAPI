function formatDate(dateString) {
    var date = new Date(dateString);
    var month = parseInt(date.getMonth()) + 1;
    return date.getDate() + "." + month + "." + date.getFullYear() + " " + ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2);
};

$(document).ready(function () {
    var apiurl = 'https://rohlikapi.azurewebsites.net/api/GetAllProducts';
    $.get(apiurl, function (data) {
        var syncTime = data.SyncTime;
        var products = data.Products;
        var dataSet = [];
        products.forEach(function (item) {
            item.Size = (item.Price / item.PPU).toFixed(2);
            dataSet.push(
                [item.Img, item.Name, item.Price, item.PPU, item.Size, item.Unit, item.Href, item.Sname]
            );
        }, this);

        var time = formatDate(data.SyncTime);


        $('#products').DataTable({
            order: [[3, "asc"]],
            data: dataSet,
            columns: [
                { title: "Image" },
                { title: "Name" },
                { title: "Price" },
                { title: "Price per unit" },
                { title: "Size" },
                { title: "Unit" },
                { title: "Link" },
                { title: "Sname" },
            ],
            columnDefs: [
                { targets: 1, searchable: true },
                { targets: '_all', searchable: false },
                { targets: [6, 7], visible: false },
                {
                    targets: 0,                    
                    data: "img",
                    sortable: false,
                    render: function (url, type, full) {
                        return '<img class="lazy" data-original="' + full[0] + '"/>';
                    }
                },
                {
                    targets: 1,
                    searchable: false,
                    data: "img",
                    render: function (url, type, full) {
                        return '<a href=' +full[6]+ '>'+full[1] +'</a>';
                    }
                }
            ],
            bLengthChange: false,
            iDisplayLength: 100,
            drawCallback: function () {
                $("img.lazy").lazyload();
            }
        });
    });
});