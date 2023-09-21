import {fetchGet, fetchPost, getSearch} from "./QueryHelper";
import { UserService } from "./UserService";

export class ReviewingService {

    constructor(mgr) {
        this.userService = new UserService();
        this.userManager = mgr;
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

    getSubjects = async (startWith) => {
        const params = [
            { name: 'startWith', value: startWith }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/subjects' + getSearch(params);
        return await fetchGet(url);
    }

    getMostPopularTags = async (pageSize) => {
        const params = [
            { name: 'pageSize', value: pageSize}
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/tags/popular' + getSearch(params);
        return await fetchGet(url);
    }

    getReview = async (reviewId, userId = null) => {
        const params = [
            { name: 'userId', value: userId }
        ]
        
        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/' + reviewId + getSearch(params);
        return await fetchGet(url);
    }

    getShortReviewsDescriptions = async (pageSize, pageNumber, sortOptions, filterOptions, tags, withUsernames = false, userId = null) => {
        const params = [
            { name: 'pageSize', value: pageSize },
            { name: 'pageNumber', value: pageNumber },
            { name: 'sortOptions', value: sortOptions },
            { name: 'filterOptions', value: filterOptions },
            { name: 'tags', value: tags },
            { name: 'userId', value: userId }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews' + getSearch(params);
        const reviews = await fetchGet(url);

        if (reviews) {
            if (withUsernames) {
                const userIds = reviews.map((review) => review.authorUserId)
            
                const userNames = await this.userService.getUserNames(userIds)
                const reviewsWithAuthors = reviews.map((review) => {
                    const userName = userNames.filter(x => x.id?.toLowerCase() === review.authorUserId);
                    review.userName = userName[0]?.userName;
                    return review;
                });
                return reviewsWithAuthors;
            }

            return reviews;
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

    saveReview = async (review, image) => {
        const url = process.env.REACT_APP_REVIEWING_API + '/reviews';
        let body = review;
        body.image = image?.dataURL ?? null;
        body.imageType = image?.file?.type ?? null;
        const user = await this.userManager.getUser();
        const token = user?.access_token;
        if (!token) return;
        const headers = {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        }

        const response = await fetchPost(url, body, headers);
        return response;
    }

    likeReview = async (reviewId, userId) => {
        const params = [
            { name: 'reviewId', value: reviewId },
            { name: 'userId', value: userId }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/like' + getSearch(params);
        const user = await this.userManager.getUser();
        const token = user?.access_token;
        if (!token) return;
        const headers = {
            "Authorization": "Bearer " + token
        }

        const response = await fetchPost(url, {}, headers);
        return response;
    }

    estimateReview = async (reviewId, userId, grade) => {
        const params = [
            { name: 'reviewId', value: reviewId },
            { name: 'userId', value: userId },
            { name: 'grade', value: grade }
        ]

        const url = process.env.REACT_APP_REVIEWING_API + '/reviews/estimate' + getSearch(params);
        const user = await this.userManager.getUser();
        const token = user?.access_token;
        if (!token) return;
        const headers = {
            "Authorization": "Bearer " + token
        }

        const response = await fetchPost(url, {}, headers);
        return response;
    } 
}