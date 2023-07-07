//Ref: https://www.javascripttutorial.net/javascript-dom/javascript-infinite-scroll/
(function () {
    const quotesEl = document.querySelector('.quotes');
    const limit = 10;
    let currentPage = 1;
    let total = 0;
    let alreadyFetching = false;

    const getQuotesDummy = (page, limit) => {
        return [{
            id: 1,
            quote: "Talk is cheap. Show me the code.",
            author: "Linus Torvalds"
        }];
    }

    const getQuotes = async (page, limit) => {
        const API_URL = `https://api.javascripttutorial.net/v1/quotes/?page=${page}&limit=${limit}`;
        let response = await fetch(API_URL);
        if (response.ok) {
            let result = await response.json();
            total = result.total;
            return result.data;
        }          
        else
            throw new Error(`An error occurred ${response.status}`);
    };

    const showQuotes = (quotes) => {
        quotes.forEach(qt => {
            let blockquoteEl = document.createElement("blockquote");
            blockquoteEl.classList.add("quote");
            blockquoteEl.innerHTML = `<span>[${qt.id}] </span> ${qt.quote} <footer>${qt.author}</footer>`;
            quotesEl.appendChild(blockquoteEl);
        });
    }
    //alert("Infinite scroll script loaded");
    
    const loadQuotes = async () => {
        setTimeout(async () => {
            try {
                if (alreadyFetching || !hasMoreQuotes()) return;
                alreadyFetching = true;
                showLoadingSign('.quotes');
                let quotesBatch = await getQuotes(currentPage, limit);
                showQuotes(quotesBatch);

                currentPage++;
            } catch (err) {
                console.log('Error occurred: ' + err.message);
            }
            alreadyFetching = false;
            hideLoadingSign('.quotes');
        }, 500);
    };

    const hasMoreQuotes = () => {
        let startIndex = (currentPage - 1) * limit + 1;
        return total == 0 || startIndex <= total;
    };
 
    loadQuotes();

    quotesEl.addEventListener('scroll', scrollCaller.bind(this), { passive: true });

    function scrollCaller() {
        let { scrollTop, scrollHeight, clientHeight } = quotesEl;
        if (scrollTop + clientHeight + 1 > scrollHeight) {
            loadQuotes();
        }
    }
})();