import '@mdxeditor/editor/style.css'
import React, { useContext, useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {ReviewingService} from "../../Services/ReviewingService";
import { Tag } from 'antd';
import { MDBContainer, MDBIcon } from "mdb-react-ui-kit";
import { UserService } from "../../Services/UserService";
import { useTranslation } from "react-i18next";
import HrStyle from "../../Assets/Css/hr";
import ReactMarkdown from 'react-markdown';
import ReviewComments from './ReviewComments';
import rangeParser from 'parse-numeric-range';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { oneLight } from 'react-syntax-highlighter/dist/cjs/styles/prism';
import { UserManagerContext } from '../../Contexts/UserManagerContext';

// markdown plugins
import remarkGfm from 'remark-gfm'
import Rating from 'react-rating';

function Review() {

    const syntaxTheme = oneLight;
    const MarkdownComponents = {
        code({ node, inline, className, ...props }) {
            const hasLang = /language-(\w+)/.exec(className || '');
            const hasMeta = node?.data?.meta;

            const applyHighlights = (applyHighlights) => {
                if (hasMeta) {
                const RE = /{([\d,-]+)}/;
                const metadata = node.data.meta?.replace(/\s/g, '');
                const strlineNumbers = RE?.test(metadata)
                    ? RE?.exec(metadata)[1]
                    : '0';
                const highlightLines = rangeParser(strlineNumbers);
                const highlight = highlightLines;
                const data = highlight.includes(applyHighlights)
                    ? 'highlight'
                    : null;
                return { data };
                } else {
                return {};
                }
            };

            return hasLang ? (
                <SyntaxHighlighter
                    style={syntaxTheme}
                    language={hasLang[1]}
                    PreTag="div"
                    className="codeStyle"
                    showLineNumbers={true}
                    wrapLines={hasMeta}
                    useInlineStyles={true}
                    lineProps={applyHighlights}
                    >
                    {props.children}
                </SyntaxHighlighter>
            ) : (
                <code className={className} {...props} />
            )
        },
    }

    const { id } = useParams();

    const mgr = useContext(UserManagerContext);
    const [userId, setUserId] = useState('');
    const [isLiked, setIsLiked] = useState(false);
    const [estimate, setEstimate] = useState(0);
    const [likesCount, setLikesCount] = useState(0);
    const reviewingService = useMemo(() => new ReviewingService(mgr), [mgr]);
    const userService = useMemo(() => new UserService(), []);
    const [review, setReview] = useState(null);
    const [author, setAuthor] = useState({});

    const navigate = useNavigate();

    // translation
    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

    const like = () => {
        if (!userId) return;
        if (review?.authorUserId === userId) return;
        reviewingService.likeReview(review?.id, userId)
            .then(response => {
                if (response.result) {
                    setIsLiked(current => !current);
                    isLiked ? setLikesCount(current => current - 1) : setLikesCount(current => current + 1)
                }
                    

            });
    }

    const estimateReview = (grade) => {
        reviewingService.estimateReview(review.id, userId, grade);
        setEstimate(grade);
    }

    /* eslint-disable */
    useMemo(() => {
        i18n.isInitialized &&
        !i18n.hasLoadedNamespace(ns) && 
            i18n.loadNamespaces(ns)
            .then(() => {
                setPageLoadingStage(false);
            });
        i18n.isInitialized &&
        i18n.hasLoadedNamespace(ns) &&
            setPageLoadingStage(false);
    }, [i18n.isInitialized]);

    useEffect(() => {
        mgr.getUser()
            .then(user => {
                let userId = null
                if (user) {
                    setUserId(user.profile.sub);
                    userId = user.profile.sub;
                }

                if (id) {
                    reviewingService.getReview(id, userId)
                        .then(review => {
                            setReview(review);
                            userService.getUser(review?.authorUserId)
                                .then(user => {
                                    setAuthor(user);
                                })
                            setIsLiked(review.isLiked ?? false);
                            setEstimate(review.userEstimate ?? 0);
                            setLikesCount(review.likesCount ?? 0);
                        });
                }
            });
    }, [])
    /* eslint-enable */

    return !review && pageLoadingStage ? '' :
    <MDBContainer >
        <h1 className="mt-5 mb-4">{review.name}</h1>

        <div className="my-4 d-flex flex-column">
            <div className="mb-2" style={HrStyle.horizontalHrStyle}/>
            <div className="d-flex flex-row justify-content-between">
                <div>
                    <span role="button" className="me-1 link-primary user-select-none" onClick={() => {navigate("/profile/" + author.id)}}>{author.userName}</span>
                    <span>&#x2022;</span>
                    <span className="mx-1">
                        {review.authorLikesCount}
                        <MDBIcon className="mx-1" icon="thumbs-up"/>
                    </span>
                </div>
                <div>
                <div className="d-flex justify-content-start">
                        <span className="mx-1">
                            {likesCount ?? 0}
                            <MDBIcon role="button" className="mx-1" icon="thumbs-up" style={isLiked ? {color: '#3b71ca'} : {}} onClick={() => like()}/>
                        </span>
                        <span>&#x2022;</span>
                        <span className="mx-1">
                            {(userId && review?.authorUserId !== userId) ?
                                <div>
                                    {review?.estimate ?? 0}
                                    <Rating
                                        className='mx-1'
                                        emptySymbol={<MDBIcon far icon="star"/>}
                                        fullSymbol={<MDBIcon fas icon="star"/>}
                                        stop={5}
                                        initialRating={estimate}
                                        onChange={e => estimateReview(e)}/>
                                </div> :
                                <div>
                                    {review?.estimate ?? 0}
                                    <MDBIcon className="mx-1" icon="star"/>
                                </div>}
                        </span>
                        {review?.publishedDate && <span>&#x2022;</span>}
                        {review?.publishedDate &&<span className="mx-1">{t('published-date', { date: review?.publishedDate ?? ''})}</span>}
                    </div>
                </div>
            </div>
            <div className="mt-2" style={HrStyle.horizontalHrStyle}/>
        </div>

        <div className="d-flex justify-content-center mb-4"><img style={{maxHeight: "300px"}} src={review.imageUrl ?? "https://placehold.co/200x200?text=No+Image"} alt="..."/></div>

        <div className="d-flex flex-column">
            <div className="my-2" style={HrStyle.horizontalHrStyle}/>
            <div className="d-flex flex-row">
                <span>{review.subjectName}</span>
                <span className="mx-1">&#x2022;</span>
                <span>{review.subjectGroupName}</span>
                <span className="mx-1">&#x2022;</span>
                <span>
                    {review.subjectGrade}
                    <MDBIcon className="mx-1" icon="star"/>
                </span>
            </div>
            <div className="my-2" style={HrStyle.horizontalHrStyle}/>
        </div>

        <div className='d-flex flex-column my-5'>
            <ReactMarkdown
                components={MarkdownComponents}
                remarkPlugins={[remarkGfm]} 
                children={(review?.content || '')} /> 
        </div>

        {
            review?.tags?.length > 0 &&
            <div className="mb-4">
                <div className="mb-2" style={HrStyle.horizontalHrStyle}/>
                {review?.tags?.map(t => <Tag color="#55acee" key={t} className="mx-1">{t}</Tag>)}
                <div className="mt-2" style={HrStyle.horizontalHrStyle}/>
            </div>
        }
        <div>
            <ReviewComments />
        </div>
    </MDBContainer>
}

export default Review;