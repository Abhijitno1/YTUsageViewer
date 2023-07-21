function InfiniteScroller(containerEl) {
    console.log('InfiniteScrollGeneric invoked');
    let currentPage = 1;
    let total = 0;
    let alreadyFetching = false;

    var exposed = {
        limit: 10,
        getDataApiUrl: '',
        showFetchedData: function () { },
        loadAdditionalData: async () => {
            setTimeout(async () => {
                try {
                    if (alreadyFetching || !hasMoreData()) return;
                    alreadyFetching = true;
                    showLoadingSign(containerEl);
                    let quotesBatch = await FetchNextPageData(currentPage);
                    exposed.showFetchedData(quotesBatch);

                    currentPage++;
                } catch (err) {
                    console.log('Error occurred: ' + err.message);
                }
                alreadyFetching = false;
                hideLoadingSign(containerEl);
            }, 500);
        }
    };

    async function FetchNextPageData(page) {

        let response = await fetch(exposed.getDataApiUrl + `?page=${page}&limit=${exposed.limit}`);
        if (response.ok) {
            let result = await response.json();
            total = result.total;
            return result.data;
        }
        else
            throw new Error(`An error occurred ${response.status}`);
    };

    function hasMoreData() {
        let startIndex = (currentPage - 1) * exposed.limit + 1;
        return total == 0 || startIndex <= total;
    };

    containerEl.addEventListener('scroll', scrollCaller.bind(this), { passive: true });

    function scrollCaller() {
        let { scrollTop, scrollHeight, clientHeight } = containerEl;
        if (scrollTop + clientHeight + 1 > scrollHeight) {
            exposed.loadAdditionalData();
        }
    }

    return exposed;
}
function test() {
    console.log('it worked this way');
}