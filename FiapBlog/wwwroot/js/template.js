define([], function () {

    function generateBlogItem(item) {
        var template = $('#blog-card').html();
        template = template.replace('{{PostId}}', item.postId);
        template = template.replace('{{Title}}', item.title);
        template = template.replace('{{ShortDescription}}', item.shortDescription);
        template = template.replace('{{Link}}', item.link);
        return template;
    }

    function appendBlogList(items) {
        var cardHtml = '';
        for (var i = 0; i < items.length; i++) {
            cardHtml += generateBlogItem(items[i]);
        }

        $('.blog-list').append(cardHtml);
    }

    function showBlogItem(html, link) {
        var template = $('#blog-item').html();
        template = template.replace('{{Link}}', link);
        template = template.replace('{{Content}}', html);
        $('#blog-item-container').html(template);
    }

    return {
        appendBlogList: appendBlogList,
        showBlogItem: showBlogItem
    }
});