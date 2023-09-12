import {fetchGet, getSearch} from "./QueryHelper";
import { UserService } from "./UserService";

export class ReviewingService {

    constructor(/*mgr*/) {
        this.userService = new UserService();
        // this.userManager = mgr;
    }

    getTags = async (startWith, pageSize, pageNumber) => {
        const params = [
            { name: 'pageSize', value: pageSize },
            { name: 'pageNumber', value: pageNumber },
            { name: 'startWith', value: startWith }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/tags' + getSearch(params);
        return await fetchGet(url);
    }

    getReview = async (reviewId) => {
        if (!reviewId) return;

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/' + reviewId;
        return await fetchGet(url);
    }

    getShortReviewsDescriptions = async (pageSize, pageNumber, sortOptions, filterOptions, tags) => {
        const params = [
            { name: 'pageSize', value: pageSize },
            { name: 'pageNumber', value: pageNumber },
            { name: 'sortOptions', value: sortOptions },
            { name: 'filterOptions', value: filterOptions },
            { name: 'tags', value: tags },
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews' + getSearch(params);
        // const token = (await this.userManager?.getUser())?.access_token;
        // const headers = token && { "Authorization": "Bearer " + token };
        const reviews = await fetchGet(url); //headers ? headers : {}

        if (reviews) {
            const userIds = reviews.map((review) => review.authorUserId)
            const userNames = await this.userService.getUserNames(userIds)
            const reviewsWithAuthors = reviews.map((review) => {
                const userName = userNames.filter(x => x.id?.toLowerCase() === review.authorUserId);
                review.userName = userName[0]?.userName;
                return review;
            });
            return reviewsWithAuthors;
        }

        return [];
    };

    getCount = async (filterOptions, tags) => {
        const params = [
            { name: 'filterOptions', value: filterOptions },
            { name: 'tags', value: tags },
        ]

        //const user = await this.userManager.getUser();
        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/count' + getSearch(params);
        const response = await fetchGet(url);
        const count = response[0].reviewsCount;
        return count;
    }
}