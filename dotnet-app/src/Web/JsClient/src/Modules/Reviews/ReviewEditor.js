import React, { useContext, useEffect, useMemo, useState } from "react";
import { 
    BlockTypeSelect, 
    BoldItalicUnderlineToggles, 
    CreateLink, 
    DiffSourceToggleWrapper,
    ListsToggle, 
    MDXEditor, 
    Separator, 
    UndoRedo,
    CodeToggle,
    InsertCodeBlock,
    diffSourcePlugin, 
    frontmatterPlugin, 
    headingsPlugin, 
    linkDialogPlugin, 
    linkPlugin, 
    listsPlugin,
    markdownShortcutPlugin,
    quotePlugin,
    tablePlugin,
    thematicBreakPlugin,
    toolbarPlugin,
    codeBlockPlugin,
    codeMirrorPlugin
} from "@mdxeditor/editor";
import '@mdxeditor/editor/style.css';
import { MDBContainer, MDBIcon } from "mdb-react-ui-kit";
import { useNavigate, useParams } from "react-router";
import { ReviewingService } from "../../Services/ReviewingService";
import DropdownInput from "./DropdownInput";
import { useTranslation } from "react-i18next";
import { Tag } from "antd";
import { UserManagerContext } from "../../Contexts/UserManagerContext";
import ImageUploading from 'react-images-uploading';
import SubjectEditor from "./SubjectEditor";
import Rating from "react-rating";

function ReviewEditor() {
    const styles = {
        a: {
            cursor: 'default',
            userSelect: 'none'
        },
        tagsArea: {
            height: '14rem'
        }
    }

    const { id } = useParams();
    const { userId } = useParams();
    const [review, setReview] = useState({});
    const mgr = useContext(UserManagerContext);
    const reviewingService = useMemo(() => new ReviewingService(mgr), [mgr]);
    const [subject, setSubject] = useState({});
    const [authorSubjectGrade, setAuthorSubjectGrade] = useState(0);

    const navigate = useNavigate();

    const [dataLoading, setDataLoading] = useState(true);

    const [style, setStyle] = useState(getTheme() ?? 'light');
    const [styleClasses, setStyleClasses] = useState(getEditorClasses());

    function getTheme() {
        return JSON.parse(localStorage.getItem('theme'))?.key;
    }

    const handleStyleChanged = () => {
        const theme = getTheme();
        theme && setStyle(theme);
    }

    function getEditorClasses() {
        if (!style || style === 'light') {
            return 'light-theme light-editor';
        }

        return 'dark-theme dark-editor';
    }

    /* eslint-disable */
    useEffect(() => {
        setStyleClasses(getEditorClasses());
    }, [style])

    useEffect(() => {
        window.addEventListener('theme', handleStyleChanged);
        handleStyleChanged();
        getTags('');

        if (id) {
            reviewingService.getReview(id)
                .then(review => {
                    if (review)
                        setReview(review);
                    setDataLoading(false);
                });
        } else {
            review.authorUserId = userId;
            review.content = '# Start type here';
            setDataLoading(false);
        }

        return () => {
            window.removeEventListener('theme', handleStyleChanged);
        }
    }, [])
    /* eslint-enable */

    const markdownOnChange = (e) => {
        setReview(current => {
            let review = current;
            review.content = e;
            return review;
        });
    }

    // tags
    const [tags, setTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);

    const setTagsFilter = (selTags) => {
        setSelectedTags(selTags);
    }
    const addTag = (key) => {
        if (!selectedTags.find(tagKey => tagKey === key))
        {
            let selTags = [...selectedTags];
            selTags.push(key);
            selTags.sort();
            setTagsFilter(selTags);
            setReview(current => {
                let review = current;
                review.tags = selTags;
                return review;
            });
        }
    }

    const removeTag = (key) => {
        setSelectedTags(selectedTags.filter(t => t !== key.tag));
    }

    const getTags = (startWith, pageSize = 20, pageNumber = 0) => {
        reviewingService.getTags(startWith, pageSize, pageNumber)
        .then((tags) => {
            setTags(tags ?? []);
        });
    }

    const filterTags = () => {
        return tags?.filter(tag => !selectedTags.includes(tag));
    }

    // translation
    const ns = "reviews";
    const { t, i18n } = useTranslation(ns);
    const [pageLoadingStage, setPageLoadingStage] = useState(true);

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
    /* eslint-enable */

    // image
    const [previewImage, setPreviewImage] = useState([]);

    const onImageChanged = (images) => {
        setPreviewImage(images);
    }

    // action buttons
    async function save() {
        setReview(current => {
            let review = current;
            review.subjectId = subject?.id;
            review.subjectName = subject?.name;
            review.subjectGrade = authorSubjectGrade;
            return review;
        });
        return await reviewingService.saveReview(review, previewImage[0] ?? null);
    };

    const saveAndPublish = () => {
        setReview(current => {
            let review = current;
            review.status = 'published';
            return review;
        });
        save().then(() => {
            navigate("/profile/" + (userId ? userId : review.authorUserId));
        });
    };

    // const saveAsDraft = () => {
    //     setReview(current => {
    //         let review = current;
    //         review.status = 'draft';
    //         return review;
    //     });
    //     save().then(() => {
    //         navigate("/profile/" + (userId ? userId : review.authorUserId));
    //     });
    // };

    const cancel = () => {
        navigate("/profile/" + (userId ? userId : review.authorUserId));
    };

    // render
    return (dataLoading || pageLoadingStage) ? '' :
        <MDBContainer>
            <div className="d-flex flex-column mt-5">
                <div className="d-flex flex-row">
                    <div className="col-6">
                        <ImageUploading
                            value={previewImage}
                            onChange={onImageChanged}
                        >
                            {
                                ({
                                    onImageUpload,
                                    onImageRemove,
                                    isDragging,
                                    dragProps
                                }) => (
                                    <div className="upload__image-wrapper">
                                        <div className="d-flex justify-content-center">
                                            <div 
                                                {...dragProps}
                                                className="shadow-sm d-flex justify-content-center align-items-center" 
                                                style={{
                                                    height: '14rem',
                                                    width: '14rem',
                                                    backgroundColor: isDragging ? 'gray' : ''
                                                }}
                                            >
                                                {isDragging ? "Drop here please" : "Upload space"}
                                            </div>
                                        </div>
                                        <div className="d-flex justify-content-evenly mt-3">
                                            <button className="btn btn-primary" onClick={onImageUpload}>Upload</button>
                                            <button className="btn btn-danger" onClick={onImageRemove}>Clear</button>
                                        </div>
                                    </div>
                                )
                            }
                        </ImageUploading>
                    </div>
                    <div className="col-6 d-flex justify-content-center">
                        <img 
                            width={250} 
                            height={250}
                            alt="Preview"
                            src={previewImage?.length > 0 ? previewImage[0]?.dataURL : 'https://placehold.co/250x250?text=No+Image'}/>
                    </div>
                </div>
                <div className="mt-5 d-flex flex-row justify-content-start align-items-baseline">
                    <div className="col-6">
                        <div className="m-3">
                            <DropdownInput items={filterTags()} getItems={getTags} addItem={addTag} inputPlaceholder={t('filter_tags_input_placeholder')} />
                            <div style={styles.tagsArea} className="overflow-auto">
                            {
                                selectedTags.map(tag =>
                                <span
                                    style={styles.a}
                                    className="text-decoration-none"
                                    key={tag}
                                    onClick={() => removeTag({tag})}>
                                        <Tag className="m-1" color="#55acee">
                                            {tag}
                                        </Tag>
                                </span>)
                            }
                            </div>
                        </div>
                    </div>
                    <div className="col-6">
                        <div className="m-3 d-flex flex-column justify-content-center">
                            <SubjectEditor subject={subject} setSubject={setSubject}/>
                            <div className="d-flex flex-column justify-content-center align-items-center">
                                <Rating
                                    emptySymbol={<MDBIcon far icon="star"/>}
                                    fullSymbol={<MDBIcon fas icon="star"/>}
                                    stop={10}
                                    onChange={e => setAuthorSubjectGrade(e)}/>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="col-12">
                    <h2>Name</h2>
                    <input type="text" className="w-100 mb-3" onChange={e => {
                        setReview(current => {
                            let review = current;
                            review.name = e.target.value;
                            return review;
                        })
                    }}/>
                    <h2>Short description</h2>
                    <textarea className="w-100" title="Short description" rows={8} onChange={e => {
                        setReview(current => {
                            let review = current;
                            review.shortDesc = e.target.value;
                            return review;
                        })
                    }}></textarea>
                </div>
            </div>
            
            <div className="py-5">
                <MDXEditor
                    className={styleClasses}
                    contentEditableClassName="editor-content"
                    markdown={review?.content ?? '# Start type here'}
                    onChange={markdownOnChange}
                    plugins={[
                        toolbarPlugin({
                        toolbarContents: () => (
                            <>
                                <DiffSourceToggleWrapper>
                                    <UndoRedo />
                                    <BoldItalicUnderlineToggles />
                                    <ListsToggle />
                                    <Separator />
                                    <BlockTypeSelect />
                                    <CreateLink />
                                    <Separator />
                                    <CodeToggle/>
                                    <InsertCodeBlock/>
                                    <Separator />
                                </DiffSourceToggleWrapper>
                            </>
                        )
                        }),
                        listsPlugin(),
                        quotePlugin(),
                        headingsPlugin(),
                        linkPlugin(),
                        linkDialogPlugin(),
                        tablePlugin(),
                        thematicBreakPlugin(),
                        frontmatterPlugin(),
                        codeBlockPlugin({ defaultCodeBlockLanguage: 'txt' }),
                        codeMirrorPlugin({ codeBlockLanguages: { js: 'JavaScript', css: 'CSS', txt: 'text', jsx: 'jsx', tsx: 'tsx' } }),
                        diffSourcePlugin({ viewMode: 'rich-text', diffMarkdown: review?.content ?? 'Start type here' }),
                        markdownShortcutPlugin()
                    ]}
                />
            </div>

            <div className="d-flex justify-content-between mb-5">
                {
                    id ? 
                    <button className="btn btn-primary" onClick={() => save()}>Update</button> :
                    <div className="btn-group">
                        <button className="btn btn-primary" onClick={() => saveAndPublish()}>Save & Publish</button>
                        {/* <button className='btn btn-secondary' onClick={() => saveAsDraft()}>Save as draft</button> */}
                    </div>
                }
                <button className="btn btn-danger" onClick={() => cancel()}>Cancel</button>
            </div>
        </MDBContainer>
}

export default ReviewEditor;